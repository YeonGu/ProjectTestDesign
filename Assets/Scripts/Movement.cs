using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Ability")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private int maxJumpTimes;

    [Space] [Header("Status")] 
    [SerializeField] private bool canWalk;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isWalking;
    [SerializeField] private int jumpCount;

    private float inputX, inputY;
    private Vector2 finalVelocity;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collision collision;
    private PlayerAnime anime;
    private Attack _attack;

    private void Start()
    {
        _attack = GetComponent<Attack>();
        anime = GetComponent<PlayerAnime>();
        collision = GetComponent<Collision>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        canWalk = true;
        jumpCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //initialize
        finalVelocity = rb.velocity;
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        
        if (rb.velocity.y > 0) canWalk = false;
        else canWalk = true;
        if (canWalk)
        {
            Walk(walkSpeed);
        }
        if (Input.GetButtonDown("Jump") && jumpCount<maxJumpTimes)
        {
            Jump(jumpSpeed);
            jumpCount++;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Dash(transform.position, (Vector2)transform.position+Vector2.right,_attack.dashDistance);
        }
        rb.velocity = finalVelocity;
        Flip();
    }
    
    //Components
    private void Walk(float spd)
    {
        Vector2 v = rb.velocity;
        v.x = inputX*spd;
        finalVelocity = v;
        
        anime.SetRun(v.x != 0 && collision.isGrounded);
    }

    private void Jump(float jumpSpd)
    {
        finalVelocity.y = jumpSpd;
        print("jump");
    }

    public void Dash(Vector2 ori,Vector2 target, float distance)
    {
        Vector2 direction = target - ori;
        direction.Normalize();
        Vector2 delta = direction * distance;
        transform.position = (Vector2)transform.position + delta;
        //finalVelocity.x = 50f;
    }

    private void Flip()
    {
        //flip control
        if (inputX > 0) sprite.flipX = false;
        if (inputX < 0) sprite.flipX = true;
    }

    public void ResetJump()
    {
        jumpCount = 0;
    }
    
}
