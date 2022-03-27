using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private InputData keyBindData;
#pragma warning restore 649
    #endregion
    
    #region Getters

    public InputData KeyBindData => keyBindData;
    public List<KeyCode> skillKeyCodes = new List<KeyCode>(5);
    public float HorizontalAxis => _character.IsAnimationLocked ? 0 : Input.GetAxisRaw(keyBindData.horizontalAxis);
    public float VerticalAxis => _character.IsAnimationLocked ? 0 : Input.GetAxisRaw(keyBindData.verticalAxis);
    public bool IsInputMoving => HorizontalAxis != 0 || VerticalAxis != 0;
    public float ZoomAxis => _character.IsAnimationLocked ? 0 : Input.GetAxisRaw(keyBindData.mouseWheelAxis);
    public float MouseXAxis => _character.IsAnimationLocked ? 0 : Input.GetAxisRaw(keyBindData.mouseXAxis);
    public float MouseYAxis => _character.IsAnimationLocked ? 0 : Input.GetAxisRaw(keyBindData.mouseYAxis);
    public bool GetJumpInput => !_character.IsAnimationLocked && Input.GetKeyDown(keyBindData.jump);
    public bool GetChangeSpeedInput => !_character.IsAnimationLocked && Input.GetKey(keyBindData.changeSpeed);
    public bool GetSwitchCharacterInput => !_character.IsAnimationLocked && Input.GetKeyDown(keyBindData.switchCharacter);
    public bool GetInteractInput => !_character.IsAnimationLocked && Input.GetKeyDown(keyBindData.interact);
    public bool GetPauseInput => Input.GetKeyDown(keyBindData.pause);
    public bool GetSkill1Input => Input.GetKeyDown(keyBindData.skill1);
    public bool GetSkill2Input => Input.GetKeyDown(keyBindData.skill2);
    public bool GetSkill3Input => Input.GetKeyDown(keyBindData.skill3);
    public bool GetSkill4Input => Input.GetKeyDown(keyBindData.skill4);
    public bool GetSkill5Input => Input.GetKeyDown(keyBindData.skill5);

    #endregion
    
    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
        skillKeyCodes.Insert(0,keyBindData.skill1);
        skillKeyCodes.Insert(1,keyBindData.skill2);
        skillKeyCodes.Insert(2,keyBindData.skill3);
        skillKeyCodes.Insert(3,keyBindData.skill4);
        skillKeyCodes.Insert(4,keyBindData.skill5);
    }

    private void Start()
    {
        GameStatus.ChangeGameStatus(GameState.Playing);
    }
}
