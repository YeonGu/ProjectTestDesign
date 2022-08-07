using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorControl : MonoBehaviour
{
    private EnemyIndicator indicator;
    // Start is called before the first frame update
    void Start()
    {
        indicator = GetComponentInChildren<EnemyIndicator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
