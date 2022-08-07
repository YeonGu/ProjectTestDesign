using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Soldier : MonoBehaviour
{
    [SerializeField] private Transform rangeIndicator;

    [Header("Ability")] 
    [SerializeField] private float normalAttackDistance;
    [SerializeField] private float speedAfterDash;
    [SerializeField] private float distanceNoCharge;
    [FormerlySerializedAs("dashAttackDistance")] 
    [SerializeField] private float distanceCharged;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float staySecondsAfterDash;

    [Space] [Header("Property")] 
    [SerializeField] private float dashChargeTime;

    private LayerMask enemyMask;
    private bool collided;

    private Movement movement;
    private Rigidbody2D rb;
    private Collision collision;

    private float currentChargeRadius;
    // Start is called before the first frame update
    void Start()
    {
        collision = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
        enemyMask=LayerMask.GetMask("Enemy");
        
        rangeIndicator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        collided = collision.collided;
        IndicateEnemy(GetNearEnemy(distanceNoCharge));
        if (Input.GetButtonDown("Fire1"))
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

/*
    IEnumerator DashTo(GameObject target,float speed)
    {
        Vector2 tarPos, pos;
        tarPos = target.transform.position;
        pos = transform.position;
        while (Vector2.Distance(tarPos,pos) > 0.5f)
        {
            tarPos = target.transform.position;
            pos = transform.position;
            movement.SetMove(false);
            Vector2 direction = tarPos - pos;
            direction.Normalize();
            transform.position =(Vector3)(direction * speed * Time.deltaTime)+ transform.position;
            yield return null;
        }

        var dp = tarPos - pos;
        transform.position = transform.position + (Vector3)(dp * 2.2f);
        rb.velocity = dp.normalized * dashSpeed;
        yield return new WaitUntil(() => collided);
        movement.SetMove(true);
        yield break;
    }
*/

    IEnumerator ChargeTimer()
    {
        //很短的时间之内 直接释放
        yield return new WaitForSeconds(dashChargeTime);
        if (!Input.GetButton("Fire1"))
        {
            QuickDash(GetNearEnemy(distanceNoCharge));
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
            if (currentChargeRadius>=distanceCharged)
            {
                break;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                print("充能过程中释放");
                rangeIndicator.gameObject.SetActive(false);
                yield break;
            }
        }
        yield return new WaitUntil(() => Input.GetButtonUp("Fire1"));
        rangeIndicator.gameObject.SetActive(false);
        print("充能完成后释放");
        yield break;
        
    }

    private void QuickDash(Collider2D target)
    {
        if (!target) return;
        Vector2 targetPos = target.transform.position;
        Vector2 selfPos = transform.position;
        Vector2 direction = targetPos - selfPos;
        float posFix = GetComponent<CapsuleCollider2D>().size.x + target.bounds.size.x;
        posFix /= 1.7f;
        direction += (direction.normalized * posFix);
        transform.position = transform.position + (Vector3)direction;
        
        StayInAir(staySecondsAfterDash);
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
        float arrowVerOffset = 0.3f;
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
}
