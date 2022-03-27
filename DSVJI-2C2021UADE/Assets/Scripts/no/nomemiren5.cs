using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class nomemiren5 : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private Image fillHp;
    [SerializeField] private Health enemyHp;
    [SerializeField] private TextMeshProUGUI hpText;
    
#pragma warning restore 649

    #endregion

    private void Start()
    {
        UpdateHealth();
        enemyHp.OnConsumed += UpdateHealth;
    }

    private void UpdateHealth()
    {
        fillHp.fillAmount = enemyHp.GetRatio;
        hpText.text = $"{enemyHp.CurrentHealth}";
    }
}
