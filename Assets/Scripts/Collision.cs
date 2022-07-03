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
    
    [Space] 
    public bool groundLeave;
    public bool groundTouch;

    private bool justGrounded;

    // Start is called before the first frame update
    void Start()
    {
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
        justGrounded = isGrounded;  //Last Frame
        //basic detect
        Vector2 ori = transform.position;
        isGrounded = Physics2D.OverlapBox(ori + bottomOffset, bottomSize, 0,3);
        rightCollided = Physics2D.OverlapBox(ori + rightOffset, rightSize, 0,3);
        leftCollided = Physics2D.OverlapBox(ori + leftOffset, leftSize, 0,3);

        groundTouch = !justGrounded && isGrounded;
        groundLeave = justGrounded && !isGrounded;
        inAir = !isGrounded && !rightCollided && !leftCollided;
    }

    private void OnDrawGizmos()
    {
        Vector2 ori = transform.position;
        Gizmos.DrawWireCube(ori+bottomOffset,bottomSize);
        Gizmos.DrawWireCube(ori+leftOffset,leftSize);
        Gizmos.DrawWireCube(ori+rightOffset,rightSize);
    }
}
