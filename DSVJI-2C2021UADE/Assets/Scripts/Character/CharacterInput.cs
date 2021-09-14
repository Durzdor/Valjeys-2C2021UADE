using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private List<string> skillHotkeys;
    
    [Header("Inputs")] [Space(5)]
    [SerializeField] private string jump = "space";
    [SerializeField] private string changeSpeed = "left shift";
    [SerializeField] private string switchCharacter = "r";
    [SerializeField] private string pause = "escape";
    [SerializeField] private string cameraLookAround = "mouse 0";
    [SerializeField] private string cameraPlayerControl = "mouse 1";
#pragma warning restore 649
    #endregion

    private Character character;
    
    public float HorizontalAxis => character.IsAnimationLocked ? 0 : Input.GetAxis("Horizontal");
    public float VerticalAxis => character.IsAnimationLocked ? 0 : Input.GetAxis("Vertical");
    public float StrafeAxis => character.IsAnimationLocked ? 0 : Input.GetAxis("SecondaryHorizontal");
    public float ZoomAxis => character.IsAnimationLocked ? 0 : Input.GetAxis("Mouse ScrollWheel");
    public List<string> SkillHotkeys => skillHotkeys; // saves all the skill hotkeys together
    public bool GetJumpInput => !character.IsAnimationLocked && Input.GetKeyDown(jump);
    public bool GetChangeSpeedInput => !character.IsAnimationLocked && Input.GetKey(changeSpeed);
    public bool GetSwitchCharacterInput => !character.IsAnimationLocked && Input.GetKeyDown(switchCharacter);
    public bool GetPauseInput => Input.GetKeyDown(pause);
    public bool GetCameraPlayerControl =>!character.IsAnimationLocked && Input.GetKey(cameraPlayerControl);
    public bool GetCameraLookAround =>!character.IsAnimationLocked && Input.GetKey(cameraLookAround);
    

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        Cursor.visible = false;
    }
    
    public bool GetSkillHotkeyInput(int skill)
    {
        if (character.IsAnimationLocked) return false;
        if (skill >= skillHotkeys.Count || skill < 0) return false;
        return Input.GetKeyDown(skillHotkeys[skill]);
    }
}
