using System;
using System.Collections;
using UnityEngine;

public class Mana : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Max Stats")] [Space(5)] 
    [SerializeField] private float maxMana = 100f;
    [Header("Passive Regen")] [Space(5)] 
    [SerializeField] private bool passiveRegenPossible;
    [SerializeField] private float manaRegenAmount = 1f;
    [SerializeField] private float manaRegenRate = 1f;
#pragma warning restore 649
    #endregion
    
    private bool _isManaRegenerating;

    public event Action OnConsumed;
    public event Action OnGained;
    
    public float CurrentMana { get; private set; }
    public float MaxMana => maxMana;
    public float GetRatio => CurrentMana / maxMana;

    private void Awake()
    {
        CurrentMana = maxMana;
    }
    
    private void Update()
    {
        if (!passiveRegenPossible) return;
        if(CurrentMana != MaxMana && !_isManaRegenerating) 
        {
            StartCoroutine(RegainManaOverTime());
        }
    }
    private IEnumerator RegainManaOverTime() 
    {
        _isManaRegenerating = true;
        while (CurrentMana < MaxMana) 
        {
            GainMana(manaRegenAmount);
            yield return new WaitForSeconds (manaRegenRate);
        }
        _isManaRegenerating = false;
    }
    
    public void GainMana(float manaAmount)
    {
        var manaBefore = CurrentMana;
        CurrentMana += manaAmount;
        CurrentMana = Mathf.Clamp(CurrentMana, 0f, maxMana);

        // call OnGained action
        var trueManaAmount = CurrentMana - manaBefore;
        if (trueManaAmount > 0f)
        {
            // use this to update on screen
            OnGained?.Invoke();
        }
    }
   
    public void ConsumeMana(float manaConsumed)
    {
        var manaBefore = CurrentMana;
        CurrentMana -= manaConsumed;
        CurrentMana = Mathf.Clamp(CurrentMana, 0f, maxMana);

        // call OnConsumed action
        var trueManaConsumedAmount = manaBefore - CurrentMana;
        if (trueManaConsumedAmount > 0f)
        {
            // use this to display on screen
            OnConsumed?.Invoke();
        }
    }

    public void ResetToMax()
    {
        GainMana(maxMana);
    }
}