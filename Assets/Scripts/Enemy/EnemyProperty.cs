using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperty : MonoBehaviour
{
    [SerializeField] private float maxLife;
    [SerializeField] private float currentLife;

    private SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLife<=0)
        {
            EnemyDie();
        }
        
    }

    public void EnemyTakeDamage(float damage)
    {
        currentLife -= damage;
        StartCoroutine(HurtFlash());
    }

    private void EnemyDie()
    {
        gameObject.SetActive(false);
    }

    IEnumerator HurtFlash()
    {
        for (int i = 0; i < 2; i++)
        {
            renderer.material.SetFloat("_FlashAmount", 1f);
            yield return new WaitForSeconds(0.06f);
            renderer.material.SetFloat("_FlashAmount", 0);
            yield return new WaitForSeconds(0.06f);
        }
    }
}
