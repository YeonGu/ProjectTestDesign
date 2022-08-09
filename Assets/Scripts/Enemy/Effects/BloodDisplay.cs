using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class BloodDisplay : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image BloodDisplayer;
    [SerializeField] private Image HurtBuffer;

    private EnemyProperty property;
    // Start is called before the first frame update
    void Start()
    {
        property = GetComponent<EnemyProperty>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBloodTo(float value)
    {
        float ratio = value / property.GetEnemyMaxLife();
        BloodDisplayer.fillAmount = ratio;
        StartCoroutine(BloodBuffer(ratio));
    }

    IEnumerator BloodBuffer(float finalRatio)
    {
        yield return new WaitForSeconds(0.5f);
        HurtBuffer.fillAmount = finalRatio;
    }
}
