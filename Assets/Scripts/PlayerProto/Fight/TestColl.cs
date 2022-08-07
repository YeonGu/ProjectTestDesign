using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColl : MonoBehaviour
{
    private LayerMask enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy= LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        var info = Physics2D.OverlapCircle(transform.position, 3f,enemy);
        if (info)
        {
            print(info.name);
        }
    }
}
