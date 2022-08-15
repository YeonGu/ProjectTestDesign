#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMove2Point : MonoBehaviour
{
    [SerializeField] private List<Transform> initialPointSet;
    [SerializeField] private List<float> staySec;
    
    private PointSetInfo[] pointInfo;
    private short pointer;
    private bool moveFinished;

    // Start is called before the first frame update
    void Start()
    {
        float pointCnt = Mathf.Min(initialPointSet.Count, staySec.Count);  //实际上的有效点数量
        if (pointCnt<2)  //无效的移动，自我禁用
        {
            EnemyMove2Point self = GetComponent<EnemyMove2Point>();
            self.enabled = false;
        }
        for (int i = 0; i < pointCnt; i++)   //初始化point info
        {
            pointInfo[i].pointPos = initialPointSet[i];
            pointInfo[i].staySecAfterPoint = staySec[i];
        }

        pointer = 0;
        moveFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveFinished)
        {
            StartCoroutine(Move2Next());
        }            
    }

    IEnumerator Move2Next()
    {
        moveFinished = false;
        pointer++;
        if (pointer==pointInfo.Length)  pointer = 0;

        transform.DOMove(pointInfo[pointer].pointPos.position, 5f);
        yield return new WaitForSeconds(5f);
        moveFinished = true;
    }

    struct PointSetInfo
    {
        public Transform pointPos;
        public float staySecAfterPoint;
    }
}
