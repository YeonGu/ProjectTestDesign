using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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
    public bool inAir;
    public bool collided;
    
    [Space] 
    public bool groundLeave;
    public bool groundTouch;

    private bool justGrounded;
    private Movement _movement;

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
        Detection();
    }

    private void Detection()
    {
        CollisionAssignment();
        if (groundTouch)
        {
            _movement.ResetJump();
        }
    }

    private void CollisionAssignment()
    {
        justGrounded = isGrounded;  //Last Frame
        //basic detect
        Vector2 ori = transform.position;
        LayerMask mask=LayerMask.GetMask("Structure");
        isGrounded = Physics2D.OverlapBox(ori + bottomOffset, bottomSize, 0,mask);
        rightCollided = Physics2D.OverlapBox(ori + rightOffset, rightSize, 0,mask);
        leftCollided = Physics2D.OverlapBox(ori + leftOffset, leftSize, 0,mask);

        groundTouch = !justGrounded && isGrounded;
        groundLeave = justGrounded && !isGrounded;
        inAir = !isGrounded && !rightCollided && !leftCollided;
        collided = !inAir;
    }

    private void OnDrawGizmos()
    {
        Vector2 ori = transform.position;
        Gizmos.DrawWireCube(ori+bottomOffset,bottomSize);
        Gizmos.DrawWireCube(ori+leftOffset,leftSize);
        Gizmos.DrawWireCube(ori+rightOffset,rightSize);
    }
}
