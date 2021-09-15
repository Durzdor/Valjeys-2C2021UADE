using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuthSkill1 : Skill
{
    private void Update()
    {
        print(UserGameObject);
        print(SkillData);
        if (Input.GetKeyDown(KeyCode.J))
        {
            UseSkill();
        }
    }
}
