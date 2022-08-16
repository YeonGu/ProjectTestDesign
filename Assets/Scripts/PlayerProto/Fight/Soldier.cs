using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Soldier : MonoBehaviour
{
    [SerializeField] private Transform rangeIndicator;

    [Header("Ability")] 
    // [SerializeField] private float speedAfterDash;
    [SerializeField] private float distanceNoCharge;
    [FormerlySerializedAs("dashAttackDistance")] 
    [SerializeField] private float distanceCharged;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float staySecondsAfterDash;

    [Space] [Header("Property")] 
    [SerializeField] private float dashChargeTime;

    [Header("Value")]
    [SerializeField] private float dashAttackPower;
                     public float slashAttackPower;
                     public float slashAttackCD = 0.5f;

                     [Header("Anger System")]
    [SerializeField] private float maxAngerValue = 100f;
    [SerializeField] private float currentAnger;
    [SerializeField] private float dashConsume;
    [SerializeField] private float normalAttackGenerate;
    [FormerlySerializedAs("centerPosition")] 
    [SerializeField] private Transform centerTransform;

    private LayerMask enemyMask;
    private bool collided;

    private Movement movement;
    private Rigidbody2D rb;
    private Collision collision;
    private CinemachineImpulseSource impulseSource;
    private SpriteRenderer renderer;

    private float currentChargeRadius;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        collision = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
        enemyMask=LayerMask.GetMask("Enemy");
        
        rangeIndicator.gameObject.SetActive(false);

        currentAnger = 0;
    }

    // Update is called once per frame
    void Update()
    {
        collided = collision.collided;
        if(!Input.GetButton("Debug Previous"))
        {
            IndicateEnemy(GetNearEnemy(distanceNoCharge));
        }
        if (Input.GetButtonDown("Debug Previous"))
        {
            StartCoroutine(ChargeTimer());
        }
    }
    
    private void OnDrawGizmos()
    {
        var position = rangeIndicator.position;
        Gizmos.DrawWireSphere(position, distanceCharged);
        Gizmos.DrawWireSphere(position, distanceNoCharge);
    }

    IEnumerator ChargeTimer()
    {
        //很短的时间之内 直接释放
        yield return new WaitForSeconds(dashChargeTime);
        if (!Input.GetButton("Debug Previous"))
        {
            QuickDash(GetNearEnemy(distanceNoCharge),0.06f);
            yield break;
        }
        
        //仍然按着 进入蓄力
        //SCALE 为 radius 的两倍
        currentChargeRadius = distanceNoCharge;
        rangeIndicator.localScale = new Vector3(currentChargeRadius * 2, currentChargeRadius * 2);
        rangeIndicator.gameObject.SetActive(true);

        while (true)
        {
            yield return 0;
            currentChargeRadius += Time.deltaTime * chargeSpeed;
            rangeIndicator.localScale = new Vector3(currentChargeRadius * 2, currentChargeRadius * 2);
            IndicateEnemy(GetNearEnemy(currentChargeRadius));
            Time.timeScale = 0.2f;

            if (currentChargeRadius>=distanceCharged)  //充满 直接退出该循环
            {
                break;
            }
            if (Input.GetButtonUp("Debug Previous"))
            {
                Time.timeScale = 1;
                //充能过程中释放
                rangeIndicator.gameObject.SetActive(false);
                QuickDash(GetNearEnemy(currentChargeRadius) ,
                    Mathf.Lerp(0.06f,0.15f,currentChargeRadius/distanceCharged));
                yield break;
            }
        }
        
        while (true)
        {
            if (Input.GetButtonUp("Debug Previous"))  //松手 发射
            {
                Time.timeScale = 1f;
                rangeIndicator.gameObject.SetActive(false);
                break;
            }
            IndicateEnemy(GetNearEnemy(distanceCharged));
            yield return 0;
        }
        // print("充能完成后释放");
        QuickDash(GetNearEnemy(distanceCharged) ,0.35f);
    }

    private void QuickDash(Collider2D target ,float impulsePower)
    {
        if (!target) return;
        impulseSource.m_DefaultVelocity = new Vector3(impulsePower, impulsePower, 0);
        impulseSource.GenerateImpulse();
        
        Vector2 targetPos = target.transform.position;
        Vector2 selfPos = centerTransform.position;
        Vector2 direction = targetPos - selfPos;
        float posFix = GetComponent<CapsuleCollider2D>().size.x + target.bounds.size.x;
        posFix /= 2f;
        direction += (direction.normalized * posFix);
        transform.position = transform.position + (Vector3)direction;
        HurtEnemy(target,dashAttackPower);
                
        StayInAir(staySecondsAfterDash);
    }

    private void HurtEnemy(Collider2D enemyInfo, float hurtValue)
    {
        EnemyProperty enemyInterface = enemyInfo.GetComponent<EnemyProperty>();
        enemyInterface.EnemyTakeDamage(hurtValue);
    }

    private Collider2D GetNearEnemy(float searchRadius)
    {
        var position = rangeIndicator.position;
        var info = Physics2D.OverlapCircle(position, searchRadius, enemyMask);
        return info ? info : null;
    }

    private void IndicateEnemy(Collider2D info)
    {
        EnemyIndicator indicator = GetComponentInChildren<EnemyIndicator>();
        if (!info)
        {
            indicator.HideIndicator();
            return;
        }
        float arrowVerOffset = 0.6f;
        indicator.transform.position = info.transform.position + new Vector3(0, arrowVerOffset);
        indicator.ShowIndicator();
    }

    private void StayInAir(float seconds)
    {
        StartCoroutine(StayForSeconds(seconds));
    }
    IEnumerator StayForSeconds(float seconds)
    {
        float g = rb.gravityScale;
        movement.useNormalWalk = false;
        rb.gravityScale = 0;
        rb.velocity=Vector2.zero;
        yield return new WaitForSeconds(seconds);
        rb.gravityScale = g;
        movement.useNormalWalk = true;

        yield break;
    }
    IEnumerator HurtFlash()
    {
        for (int i = 0; i < 2; i++)
        {
            renderer.material.SetFloat("_FlashAmount", 1f);
            yield return new WaitForSeconds(0.06f);
            renderer.material.SetFloat("_FlashAmount", 0);
            yield return new WaitForSeconds(0.06f);
        }
    }
}
