﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    /*
     * Nuevo input Template
     * 
     * [SerializeField] private string {inputName} = "key";
     * public bool Get{inputName}Input => !character.IsAnimationLocked && Input.GetKeyDown({inputName});
     * public string {InputName} => {inputName};
     * 
     * Reemplazar {inputName} con su nombre y {InputName} con su Nombre
     * despues se llama character.CharacterInput.Get{inputName}Input en el if
     */
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
    [SerializeField] private string interact = "f";
#pragma warning restore 649
    #endregion
    
    #region Bool Getters

    public float HorizontalAxis => _character.IsAnimationLocked ? 0 : Input.GetAxis("Horizontal");
    public float VerticalAxis => _character.IsAnimationLocked ? 0 : Input.GetAxis("Vertical");
    public float StrafeAxis => _character.IsAnimationLocked ? 0 : Input.GetAxis("SecondaryHorizontal");
    public float ZoomAxis => _character.IsAnimationLocked ? 0 : Input.GetAxis("Mouse ScrollWheel");
    public float MouseXAxis => _character.IsAnimationLocked ? 0 : Input.GetAxis("Mouse X");
    public float MouseYAxis => _character.IsAnimationLocked ? 0 : Input.GetAxis("Mouse Y");
    public List<string> SkillHotkeys => skillHotkeys; // saves all the skill hotkeys together
    public bool GetJumpInput => !_character.IsAnimationLocked && Input.GetKeyDown(jump);
    public bool GetChangeSpeedInput => !_character.IsAnimationLocked && Input.GetKey(changeSpeed);
    public bool GetSwitchCharacterInput => !_character.IsAnimationLocked && Input.GetKeyDown(switchCharacter);
    public bool GetInteractInput => !_character.IsAnimationLocked && Input.GetKeyDown(interact);
    public bool GetPauseInput => Input.GetKeyDown(pause);
    public bool GetCameraPlayerControlInput =>!_character.IsAnimationLocked && Input.GetKey(cameraPlayerControl);
    public bool GetCameraLookAroundInput =>!_character.IsAnimationLocked && Input.GetKey(cameraLookAround);

    #endregion

    #region String Getter

    public string Jump => jump;
    public string ChangeSpeed => changeSpeed;
    public string SwitchCharacter => switchCharacter;
    public string Pause => pause;
    public string CameraLookAround => cameraLookAround;
    public string CameraPlayerControl => cameraPlayerControl;
    public string Interact => interact;

    #endregion

    private Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private void Start()
    {
        GameStatus.ChangeGameStatus(GameState.Playing);
    }
    
    public bool GetSkillHotkeyInput(int skill)
    {
        if (_character.IsAnimationLocked) return false;
        if (skill >= skillHotkeys.Count || skill < 0) return false;
        return Input.GetKeyDown(skillHotkeys[skill]);
    }
}