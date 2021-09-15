using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour, ISkill
{
    private GameObject userGameObject;
    private SkillData skillData;

    public virtual GameObject UserGameObject
    {
        get => userGameObject;
        set => userGameObject = value;
    }

    public virtual SkillData SkillData
    {
        get => skillData;
        set => skillData = value;
    }

    public virtual void UseSkill()
    {
    }
}
