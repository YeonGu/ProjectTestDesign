using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    private SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideIndicator()
    {
        renderer.enabled = false;
    }

    public void ShowIndicator()
    {
        renderer.enabled = true;
    }
}
