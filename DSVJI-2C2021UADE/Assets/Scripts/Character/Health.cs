using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField][Tooltip("Maximum amount of health")] 
    private float maxHealth = 100f;

    public event Action OnConsumed;
    public event Action OnGained;
    public event Action OnDeath;
    
    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public float GetRatio => CurrentHealth / maxHealth;
    public float LastDamageTaken { get; private set; }
    
    public bool IsDead { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void Heal(float healAmount)
    {
        var healthBefore = CurrentHealth;
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        // call OnHeal action
        var trueHealAmount = CurrentHealth - healthBefore;
        if (trueHealAmount > 0f)
        {
            // use this to display amount healed on screen
            OnGained?.Invoke();
        }
    }
   
    public void TakeDamage(float damage)
    {
        if (IsDead) return;
        var healthBefore = CurrentHealth;
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        // call OnDamage action
        LastDamageTaken = healthBefore - CurrentHealth;
        if (LastDamageTaken > 0f)
        {
            // use this to display on screen
            OnConsumed?.Invoke();
        }

        HandleDeath();
    }
    
    private void HandleDeath()
    {
        if (IsDead) return;

        // call OnDie action
        if (CurrentHealth <= 0f)
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
    }

    public void ResetToMax()
    {
        Heal(maxHealth);
        IsDead = false;
    }
}