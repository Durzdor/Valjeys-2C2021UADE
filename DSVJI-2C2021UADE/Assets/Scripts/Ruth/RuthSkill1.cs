using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuthSkill1 : Skill
{
    [SerializeField] private SkillData data;

    private void Start()
    {
        skillData = data;
    }

    private void Update()
    {
        print(skillData);
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Play particles?
        }
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }
}
