using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
    [SerializeField] private float fallDownMultiple;
    [SerializeField] private float looseJumpMultiple;
    private Collision collision;
    private Movement movement;
    private Rigidbody2D rb;
    private float gravity=-9.81f;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        collision = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!movement.useNormalWalk)
        {
            return;
        }
        
        if (rb.velocity.y<0)
        {
            Vector2 v = rb.velocity;
            v.y = gravity * Time.deltaTime * (fallDownMultiple - 1) + v.y;
            rb.velocity = v;
        }
        
        //need fix!
        if (!Input.GetButton("Jump") && rb.velocity.y > 0)
        {
            Vector2 w = rb.velocity;
            w.y = gravity * Time.deltaTime * (looseJumpMultiple - 1) + w.y;
            rb.velocity = w;
        }
    }

    public void IncreaseJumpGravity()
    {
        StartCoroutine(JumpGra());
    }

    IEnumerator JumpGra()
    {
        if (rb.velocity.y <= 0)
        {
            yield break;
        }

        yield return new WaitUntil(() => Input.GetButtonUp("Jump"));    //松开跳跃
        while (true)
        {
            Vector2 w = rb.velocity;
            w.y = gravity * Time.deltaTime * (looseJumpMultiple - 1) + w.y;
            rb.velocity = w;
            yield return 0;

            if (collision.collided || rb.velocity.y<=0)
            {
                yield break;
            }
        }
    }
}
