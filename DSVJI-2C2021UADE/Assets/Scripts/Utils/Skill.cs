using System;
using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour, ISkill
{   
    protected SkillData Data;
    protected bool CanUseSkill;
    protected Character Character;
    private float _currentCooldown;
    private float CooldownRatio => _currentCooldown / Data.Cooldown;
    private bool HasRequiredMana => Data.UseCost < Character.Mana.CurrentMana;
    
    public SkillData SkillData => Data;
    public bool IsOffCooldown { get; private set; } = true;
    public bool WasSkillUsed => CanUseSkill;
    
    public event Action<float> OnCooldownUpdate;
    public event Action<string> OnSkillNotUsable;

    private void Awake()
    {
        Character = GetComponentInParent<Character>();
    }

    public virtual void UseSkill()
    {
        if (IsOffCooldown)
        {
            if (HasRequiredMana)
            {
                StartCoroutine(CooldownTracker());
                Character.Mana.ConsumeMana(Data.UseCost);
                CanUseSkill = true;
            }
            else
            {
                OnSkillNotUsable?.Invoke("Not enough mana.");
                CanUseSkill = false;
            }
        }
        else
        {
            OnSkillNotUsable?.Invoke("This skill is not available yet.");
            CanUseSkill = false;
        }
    }
    
    private IEnumerator CooldownTracker()
    {
        IsOffCooldown = false;
        _currentCooldown = Data.Cooldown;
        while (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
            OnCooldownUpdate?.Invoke(CooldownRatio);
            yield return null;
        }
        IsOffCooldown = true;
    }
}
