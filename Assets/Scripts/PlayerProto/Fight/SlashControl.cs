using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*实现：
 * 普攻的CD计时器
 * slash攻击的触发器（启用）
 */
public class SlashControl : MonoBehaviour
{
    [SerializeField] bool attackReady;
    // Start is called before the first frame update
    void Start()
    {
        attackReady = true;
        float ix = Input.GetAxis("Horizontal");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && attackReady)
        {
            Vector3 scale = new Vector3(1, 1, 1);
            SpriteRenderer playerRender = transform.root.GetComponent<SpriteRenderer>();
            if (playerRender.flipX)
                scale.x = -1;
            else
                scale.x = 1;
            transform.localScale = scale;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void SetCd()
    {
        float cd = transform.root.GetComponent<Soldier>().slashAttackCD;
        StartCoroutine(CdCounter(cd));
    }

    IEnumerator CdCounter(float CdSeconds)
    {
        attackReady = false;
        yield return new WaitForSeconds(CdSeconds);
        attackReady = true;
    }
}
