using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nomemiren2 : MonoBehaviour
{
    [SerializeField] private Character character;

    private void Start()
    {
        character.Ui.FadeInHandler();
        character.SkillController.UnlockNaomiSkill(0);
        character.SkillController.UnlockNaomiSkill(1);
        character.SkillController.UnlockRuthSkill(0);
        character.SkillController.UnlockRuthSkill(1);
    }
}
