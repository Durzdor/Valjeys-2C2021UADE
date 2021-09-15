using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    GameObject UserGameObject { get; set; }
    SkillData SkillData { get; set; }
    void UseSkill();
}
