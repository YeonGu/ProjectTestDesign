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

    [Space] [Header("Status")] 
    [SerializeField] private bool canWalk;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isWalking;
    

    private float inputX, inputY;
    private Vector2 finalVelocity;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collision collision;
    private PlayerAnime anime;

    private void Start()
    {
        anime = GetComponent<PlayerAnime>();
        collision = GetComponent<Collision>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        canWalk = true;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        if (canWalk)
        {
            Walk(walkSpeed);
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump(jumpSpeed);
        }

        rb.velocity = finalVelocity;

        //flip control
        if (inputX > 0) sprite.flipX = false;
        if (inputX < 0) sprite.flipX = true;
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
    }
    
}
