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

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Collision collision;

    private void Start()
    {
        collision = GetComponent<Collision>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
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

        _rigidbody.velocity = finalVelocity;

        //flip control
        if (_rigidbody.velocity.x > 0) _spriteRenderer.flipX = false;
        if (_rigidbody.velocity.x < 0) _spriteRenderer.flipX = true;
    }
    
    //Components
    private void Walk(float spd)
    {
        var v = _rigidbody.velocity;
        v.x = inputX*spd;
        finalVelocity = v;
    }

    private void Jump(float jumpSpd)
    {
        finalVelocity.y = jumpSpd;
    }
    
}
