using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Collision : MonoBehaviour
{
    [SerializeField] private Vector2 bottomOffset;
    [SerializeField] private Vector2 bottomSize;
    [SerializeField] private Vector2 leftOffset;
    [SerializeField] private Vector2 leftSize;
    [SerializeField] private Vector2 rightOffset;
    [SerializeField] private Vector2 rightSize;

    public bool isGrounded;
    public bool rightCollided;
    public bool leftCollided;
    public bool onWall;
    public bool inAir;
    public bool collided;
    
    [FormerlySerializedAs("groundLeave")] [Space] 
    public bool groundEntry;
    [FormerlySerializedAs("groundTouch")] 
    public bool groundExit;
    public bool colliderEntry;
    public bool colliderExit;

    private bool justGrounded;
    private Movement _movement;
    private bool justCollided;

    // Start is called before the first frame update
    void Start()
    {
        _movement = GetComponent<Movement>();
        justGrounded = false;
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        DetectCollide();
    }

    private void DetectCollide()
    {
        CollisionAssignment();
        if (colliderEntry)
        {
            _movement.ResetJump();
        }
    }

    private void CollisionAssignment()
    {
        justGrounded = isGrounded;  //Last Frame
        justCollided = collided;
        //basic detect
        Vector2 ori = transform.position;
        LayerMask mask=LayerMask.GetMask("Structure");
        isGrounded = Physics2D.OverlapBox(ori + bottomOffset, bottomSize, 0,mask);
        rightCollided = Physics2D.OverlapBox(ori + rightOffset, rightSize, 0,mask);
        leftCollided = Physics2D.OverlapBox(ori + leftOffset, leftSize, 0,mask);
        onWall = rightCollided || leftCollided;

        groundEntry = !justGrounded && isGrounded;
        groundExit = justGrounded && !isGrounded;
        inAir = !isGrounded && !rightCollided && !leftCollided;
        collided = !inAir;
        colliderEntry = !justCollided && collided;
        colliderExit = !collided && justCollided;

    }

    private void OnDrawGizmos()
    {
        Vector2 ori = transform.position;
        Gizmos.DrawWireCube(ori+bottomOffset,bottomSize);
        Gizmos.DrawWireCube(ori+leftOffset,leftSize);
        Gizmos.DrawWireCube(ori+rightOffset,rightSize);
    }
}
