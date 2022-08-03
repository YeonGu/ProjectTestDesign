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
    [SerializeField] private float wallFallSpeed;
    [SerializeField] private float wallSlideSpeed = -3f;

    [Space] [Header("Status")] 
    [SerializeField] private bool canWalk;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isWalking;
    [SerializeField] private int jumpCount;
    [SerializeField] private bool wallGrab;
    [SerializeField] private bool useNormalWalk;

    private float inputX, inputY;
    private Vector2 finalVelocity;
    private float defaultGravity;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collision collision;
    private PlayerAnime anime;
    private Attack attack;
    private BetterJump betterJump;

    private void Start()
    {
        betterJump = GetComponent<BetterJump>();
        attack = GetComponent<Attack>();
        anime = GetComponent<PlayerAnime>();
        collision = GetComponent<Collision>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        canWalk = true;
        jumpCount = 0;
        defaultGravity = rb.gravityScale;
        useNormalWalk = true;
    }

    // Update is called once per frame
    void Update()
    {
        Transform[] center = GetComponentsInChildren<Transform>();
        //initialize
        finalVelocity = rb.velocity;
        if (!useNormalWalk) { return; }
        GetAxis();

        //check == if On the wall, grab
        if (collision.onWall && !collision.isGrounded)
        {
            WallGrab();
            wallGrab = true;
            return;
        }

        ResetConstraints();
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
        if(canWalk)
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
        // betterJump.IncreaseJumpGravity();
    }

    private void WallGrab()
    {
        //Lock Position, g
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 0;
        var v = rb.velocity;
        
        if (inputY > 0) 
            v.y = 0;
        else if (inputY<0)
            v.y = wallFallSpeed - (wallSlideSpeed * inputY);  //负负得正
        else
            v.y = wallFallSpeed;
        
        rb.velocity = v;

        if (Input.GetButtonDown("Jump"))
        {
            canWalk = false;
            WallJump();
        }
    }

    private void WallJump()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Vector2 dir = Vector2.zero;

        if (collision.rightCollided && !collision.leftCollided)
        {
            dir = Vector2.up + Vector2.left;
        }
        if (collision.leftCollided && !collision.rightCollided)
        {
            dir = Vector2.up + Vector2.right;
        }

        StartCoroutine(WallJumpControl());
        dir.Normalize();
        transform.position =(Vector3)(dir*0.2f)+transform.position;
        
        dir *= wallJumpSpeed;
        rb.velocity = dir;
    }

    public void SetMove(bool isWalkValid)
    {
        useNormalWalk = isWalkValid;
    }

    public void Dash(Vector2 ori,Vector2 target, float distance)
    {
        Vector2 direction = target - ori;
        direction.Normalize();
        Vector2 delta = direction * distance;
        transform.position = (Vector2)transform.position + delta;
        //finalVelocity.x = 50f;
    }

    

    public void ResetJump()
    {
        jumpCount = 0;
    }
    private void Flip()
    {
        //flip control
        if (inputX > 0) sprite.flipX = false;
        if (inputX < 0) sprite.flipX = true;
    }
    private void ResetConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void GetAxis()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }

    IEnumerator WallJumpControl()
    {
        canWalk = false;
        yield return 0;
        yield return new WaitUntil(() => rb.velocity.y <= 0f);
        canWalk = true;
        yield break;
    }
}
