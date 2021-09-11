using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    [SerializeField] private List<string> skillHotkeys;
    
    [Header("Inputs")] [Space(5)]
    [SerializeField] private string jump = "space";
    [SerializeField] private string changeSpeed = "left shift";
    [SerializeField] private string switchCharacter = "r";
    [SerializeField] private string pause = "escape";

    
    
    public List<string> SkillHotkeys => skillHotkeys;

    private void Start()
    {
        Cursor.visible = false;
    }

    public bool GetJumpInput => Input.GetKeyDown(jump);
    public bool GetChangeSpeedInput => Input.GetKey(changeSpeed);
    public bool GetSwitchCharacterInput => Input.GetKeyDown(switchCharacter);
    public bool GetPauseInput => Input.GetKeyDown(pause);

    public bool GetSkillHotkeyInput(int skill)
    {
        if (skill >= skillHotkeys.Count || skill < 0) return false;
        return Input.GetKeyDown(skillHotkeys[skill]);
    }
}
