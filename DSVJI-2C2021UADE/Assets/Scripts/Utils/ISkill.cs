using System;

public interface ISkill
{
    SkillData SkillData { get; }
    bool IsOffCooldown { get; }
    event Action<float> OnCooldownUpdate;
    void UseSkill();
}
