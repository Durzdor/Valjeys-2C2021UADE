using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour, ISkill
{
    protected SkillData skillData;
    private bool isOffCooldown;
    private float currentCooldown;

    private float CooldownRatio => currentCooldown / skillData.SkillCooldown;
    public SkillData SkillData => skillData;
    public bool IsOffCooldown => isOffCooldown;
    
    public event Action<float> OnSkillCooldownUpdate;

    
    public virtual void UseSkill()
    {
        StartCoroutine(SkillCd());
    }
    
    private IEnumerator SkillCd()
    {
        isOffCooldown = false;
        while (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            OnSkillCooldownUpdate?.Invoke(CooldownRatio);
            yield return null;
        }
        isOffCooldown = true;
    }
}
