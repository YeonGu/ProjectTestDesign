using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SlashDetection : MonoBehaviour
{
    private Soldier playerInterface;
    // [SerializeField] private bool slashCd;

    // Start is called before the first frame update
    void Start()
    {
        var playerTrans = transform.root;
        playerInterface = playerTrans.GetComponent<Soldier>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.tag);

        if (other.CompareTag("Enemy"))
        {
            print(other.name);
            EnemyProperty enemyInterface = other.gameObject.GetComponent<EnemyProperty>();
            enemyInterface.EnemyTakeDamage(playerInterface.slashAttackPower);
            return;
        }
        /*switch (other.tag)
        {
            case "Enemy":
                EnemyProperty enemyProperty = other.GetComponent<EnemyProperty>();
                enemyProperty.EnemyTakeDamage(player.slashAttackPower);
                break;
            default:
                    break;
        }*/
    }
    
    //自毁 event
    private void EndSelf()
    {
        gameObject.SetActive(false);
    }
}
