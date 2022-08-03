using System;
using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetButtonDown("Fire1"))
        {
            /*var info = Physics2D.OverlapCircle(transform.position, dashAttackDistance, enemyMask);
            if (info)
            {
                StartCoroutine(DashTo(info.gameObject, speedAfterDash));
            }*/
            StartCoroutine(ChargeTimer());
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(rangeIndicator.position, distanceCharged);
        Gizmos.DrawWireSphere(rangeIndicator.position, distanceNoCharge);
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
        yield return new WaitForSeconds(dashChargeTime);
        if (!Input.GetButton("Fire1"))
        {
            print("直接释放");
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
}
