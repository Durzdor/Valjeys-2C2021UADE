using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InputData", menuName = "Input Data", order = 52)]
public class InputData : ScriptableObject
{
    public KeyCode jump = KeyCode.Space;
    public KeyCode changeSpeed = KeyCode.LeftShift;
    public KeyCode switchCharacter = KeyCode.X;
    public KeyCode interact = KeyCode.F;
    public KeyCode pause = KeyCode.Escape;
    public KeyCode skill1 = KeyCode.Mouse0;
    public KeyCode skill2 = KeyCode.Mouse1;
    public KeyCode skill3 = KeyCode.Q;
    public KeyCode skill4 = KeyCode.E;
    public KeyCode skill5 = KeyCode.R;
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string mouseXAxis = "Mouse X";
    public string mouseYAxis = "Mouse Y";
    public string mouseWheelAxis = "Mouse ScrollWheel";
    
    public KeyCode DefaultJump { get; private set; }
    public KeyCode DefaultChangeSpeed { get; private set; }
    public KeyCode DefaultSwitchCharacter { get; private set; }
    public KeyCode DefaultInteract { get; private set; }
    public KeyCode DefaultPause { get; private set; }
    public KeyCode DefaultSkill1 { get; private set; }
    public KeyCode DefaultSkill2 { get; private set; }
    public KeyCode DefaultSkill3 { get; private set; }
    public KeyCode DefaultSkill4 { get; private set; }
    public KeyCode DefaultSkill5 { get; private set; }

    private void Awake()
    {
        SaveDefaultHotkey();
    }
    
    private void SaveDefaultHotkey()
    {
        DefaultJump = jump;
        DefaultChangeSpeed = changeSpeed;
        DefaultSwitchCharacter = switchCharacter;
        DefaultInteract = interact;
        DefaultPause = pause;
        DefaultSkill1 = skill1;
        DefaultSkill2 = skill2;
        DefaultSkill3 = skill3;
        DefaultSkill4 = skill4;
        DefaultSkill5 = skill5;
    }
}
