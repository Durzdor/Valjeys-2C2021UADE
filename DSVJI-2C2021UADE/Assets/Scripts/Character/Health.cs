using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField][Tooltip("Maximum amount of health")] 
    private float maxHealth = 100f;
    
    private bool _isDead;
    
    public event Action OnConsumed;
    public event Action OnGained;
    public event Action OnDeath;
    
    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public float GetRatio => CurrentHealth / maxHealth;

    public bool IsDead => _isDead;

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
        if (_isDead) return;
        var healthBefore = CurrentHealth;
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        // call OnDamage action
        var trueDamageAmount = healthBefore - CurrentHealth;
        if (trueDamageAmount > 0f)
        {
            // use this to display on screen
            OnConsumed?.Invoke();
        }

        HandleDeath();
    }
    
    private void HandleDeath()
    {
        if (_isDead) return;

        // call OnDie action
        if (CurrentHealth <= 0f)
        {
            _isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void ResetToMax()
    {
        Heal(maxHealth);
        _isDead = false;
    }
}