using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float maxFlashDistance;

    public float dashDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 ptr = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
        GameObject obj = GameObject.FindWithTag("Enemy");
        FlashAttack(obj.transform);
    }

    void FlashAttack(Transform target)
    {
        //check 途中是否有阻挡
        var point = transform.position;
        LayerMask enemyMask=LayerMask.GetMask("Enemy");
        var flashCheck = Physics2D.RaycastAll(point, target.position - point,
            maxFlashDistance);      //Layer:enemy

        if (flashCheck.Length <= 1) return;
        foreach (var i in flashCheck)
        {
            if(i.collider.CompareTag("Structure")) return;
            if (i.collider.transform == target)
            {
                //print(111);
                break;
            }
        }

    }
}
