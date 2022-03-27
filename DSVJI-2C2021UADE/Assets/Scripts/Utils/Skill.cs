using System;
using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour, ISkill
{
    #region SerializedFields

#pragma warning disable 649
    [Header("Skill Base")][Space(5)]
    [SerializeField] protected bool useMana = true;
    [SerializeField] protected AudioSource skillAudioSource;
    [SerializeField] protected float skillAudioDelay = 0.5f;
#pragma warning restore 649

    #endregion
    protected SkillData Data;
    protected bool CanUseSkill;
    protected Character Character;
    private float _currentCooldown;
    private float CooldownRatio => _currentCooldown / Data.Cooldown;
    private bool HasRequiredMana => Data.UseCost < Character.Mana.CurrentMana;
    private bool HasRequiredStamina => Data.UseCost < Character.Stamina.CurrentStamina;
    private bool HasResource => useMana ? HasRequiredMana : HasRequiredStamina;
    
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
            if (HasResource)
            {
                StartCoroutine(CooldownTracker());
                if (useMana)
                    Character.Mana.ConsumeMana(Data.UseCost);
                else
                    Character.Stamina.ConsumeStamina(Data.UseCost);
                CanUseSkill = true;
                if (skillAudioSource != null)
                {
                    skillAudioSource.PlayDelayed(skillAudioDelay);
                }
            }
            else
            {
                OnSkillNotUsable?.Invoke($"Not enough {(useMana? "mana":"stamina")}.");
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
