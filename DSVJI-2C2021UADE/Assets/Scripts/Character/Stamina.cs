using System;
using System.Collections;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Max Stats")] [Space(5)] 
    [SerializeField] private float maxStamina = 100f;
    [Header("Passive Regen")] [Space(5)] 
    [SerializeField] private bool passiveRegenPossible;
    [SerializeField] private float staminaRegenAmount = 1f;
    [SerializeField] private float staminaRegenRate = 1f;
#pragma warning restore 649
    #endregion
    
    private bool _isStaminaRegenerating;

    public event Action OnConsumed;
    public event Action OnGained;
    
    public float CurrentStamina { get; private set; }
    public float MaxStamina => maxStamina;
    public float GetRatio => CurrentStamina / maxStamina;

    private void Awake()
    {
        CurrentStamina = maxStamina;
    }
    
    private void Update()
    {
        if (!passiveRegenPossible) return;
        if(CurrentStamina != MaxStamina && !_isStaminaRegenerating) 
        {
            StartCoroutine(RegainStaminaOverTime());
        }
    }
    private IEnumerator RegainStaminaOverTime() 
    {
        _isStaminaRegenerating = true;
        while (CurrentStamina < MaxStamina) 
        {
            GainStamina(staminaRegenAmount);
            yield return new WaitForSeconds (staminaRegenRate);
        }
        _isStaminaRegenerating = false;
    }
    
    public void GainStamina(float staminaAmount)
    {
        var staminaBefore = CurrentStamina;
        CurrentStamina += staminaAmount;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, maxStamina);

        // call OnGained action
        var trueStaminaAmount = CurrentStamina - staminaBefore;
        if (trueStaminaAmount > 0f)
        {
            // use this to update on screen
            OnGained?.Invoke();
        }
    }
   
    public void ConsumeStamina(float staminaConsumed)
    {
        var staminaBefore = CurrentStamina;
        CurrentStamina -= staminaConsumed;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, maxStamina);

        // call OnConsumed action
        var trueStaminaConsumedAmount = staminaBefore - CurrentStamina;
        if (trueStaminaConsumedAmount > 0f)
        {
            // use this to display on screen
            OnConsumed?.Invoke();
        }
    }

    public void ResetToMax()
    {
        GainStamina(maxStamina);
    }
}