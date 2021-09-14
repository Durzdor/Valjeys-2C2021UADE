using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    SkillData SkillData { get; set; }
    void UseSkill();
}
