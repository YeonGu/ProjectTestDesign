using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Ability")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] private int maxJumpTimes;
    [SerializeField] private float wallJumpSpeed;

    [Space] [Header("Status")] 
    [SerializeField] private bool canWalk;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isWalking;
    [SerializeField] private int jumpCount;
    [SerializeField] private bool wallGrab;

    private float inputX, inputY;
    private Vector2 finalVelocity;
    private float defaultGravity;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collision collision;
    private PlayerAnime anime;
    private Attack attack;

    private void Start()
    {
        attack = GetComponent<Attack>();
        anime = GetComponent<PlayerAnime>();
        collision = GetComponent<Collision>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        canWalk = true;
        jumpCount = 0;
        defaultGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        //initialize
        finalVelocity = rb.velocity;
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        
        if (collision.onWall && !collision.isGrounded)
        {
            canWalk = false;
            WallGrab();
            wallGrab = true;
            return;
        }

        if (rb.velocity.y > 0.1f) canWalk = false;
        else canWalk = true;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        wallGrab = false;
        rb.gravityScale = defaultGravity;
        if (canWalk)
        {
            Walk(walkSpeed);
        }
        if (Input.GetButtonDown("Jump") && jumpCount<maxJumpTimes)
        {
            Jump(jumpSpeed);
            jumpCount++;
        }

        /*if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Dash(transform.position, (Vector2)transform.position+Vector2.right,attack.dashDistance);
        }*/
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

    private void WallGrab()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 0;
        var v = rb.velocity;
        v.y = inputY*climbSpeed;
        rb.velocity = v;

        if (Input.GetButtonDown("Jump"))
        {
            WallJump();
        }
    }

    private void WallJump()
    {
        if (collision.rightCollided && !collision.leftCollided)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Vector2 dir = Vector2.up + Vector2.left;
            dir.Normalize();
            transform.position =(Vector3)(dir*0.2f)+transform.position;
            dir *= wallJumpSpeed;
            rb.velocity = dir;
            return;
        }

        if (collision.leftCollided && !collision.rightCollided)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Vector2 dir = Vector2.up + Vector2.right;
            dir.Normalize();
            transform.position = (Vector3) (dir * 0.2f) + transform.position;
            dir *= wallJumpSpeed;
            rb.velocity = dir;
            
        }
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
