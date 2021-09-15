using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    event Action<float> OnSkillCooldownUpdate;
    bool IsOffCooldown { get; }
    SkillData SkillData { get; }
    void UseSkill();
}
