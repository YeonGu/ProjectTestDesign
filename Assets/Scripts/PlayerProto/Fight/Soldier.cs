using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("Ability")] 
    [SerializeField] private float normalAttackDistance;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float speedAfterDash;

    [SerializeField] public float dashAttackDistance;

    private LayerMask enemyMask;
    private bool collided;

    private Movement movement;
    private Rigidbody2D rb;
    private Collision collision;
    // Start is called before the first frame update
    void Start()
    {
        collision = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
        enemyMask=LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        collided = collision.collided;
        if (Input.GetButtonDown("Fire1"))
        {
            var info = Physics2D.OverlapCircle(transform.position, dashAttackDistance, enemyMask);
            if (info)
            {
                StartCoroutine(DashTo(info.gameObject, speedAfterDash));
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, dashAttackDistance);
    }

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
}
