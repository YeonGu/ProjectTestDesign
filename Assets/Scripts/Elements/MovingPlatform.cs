using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<Transform> platformRoute;

    [SerializeField] private bool isStop;

    private int targetPoint;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = platformRoute[0].position;
        targetPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int cnt = platformRoute.Count;
        
        MoveTo(targetPoint);
    }

    private void MoveTo(int targetIndex)
    {
        Vector2 dir = platformRoute[targetIndex].position - transform.position;
        dir.Normalize();
        dir = dir * (moveSpeed * Time.deltaTime);
        transform.position = transform.position + (Vector3)dir;

        if (Vector2.Distance(platformRoute[targetIndex].position, transform.position)
                    <0.2f)
        {
            NextPoint();
        }
        return;
    }

    void NextPoint()
    {
        if (targetPoint<platformRoute.Count-1)
        {
            targetPoint++;
        }
        else
        {
            targetPoint = 0;
        }
    }
}
