using System;
using System.Collections;
using UnityEngine;

public class Mana : MonoBehaviour
{
    [Header("Max Stats")] [Space(5)] 
    [SerializeField] private float maxMana = 100f;

    [Header("Mana Regen")] [Space(5)] 
    [SerializeField] private bool passiveManaRegenPossible;
    [SerializeField] private float manaRegenAmount;
    [SerializeField] private float manaRegenRate;
    private bool isManaRegenerating;

    public event Action OnConsumed;
    public event Action OnGained;
    
    public float CurrentMana { get; private set; }
    public float MaxMana => maxMana;
    public float GetRatio => CurrentMana / maxMana;

    private void Awake()
    {
        CurrentMana = maxMana;
    }
    
    void Update()
    {
        if (!passiveManaRegenPossible) return;
        if(CurrentMana != MaxMana && !isManaRegenerating) 
        {
            StartCoroutine(RegainHealthOverTime());
        }
    }
    private IEnumerator RegainHealthOverTime() 
    {
        isManaRegenerating = true;
        while (CurrentMana < MaxMana) 
        {
            GainMana(manaRegenAmount);
            yield return new WaitForSeconds (manaRegenRate);
        }
        isManaRegenerating = false;
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
}