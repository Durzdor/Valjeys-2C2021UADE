using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAnimation : MonoBehaviour
{
    private Character character;
    
    private static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");
    private static readonly int GroundFloat = Animator.StringToHash("GroundFloat");
    private static readonly int AirFloat = Animator.StringToHash("AirFloat");
    private static readonly int IdleBool = Animator.StringToHash("Idling");
    
    private void Start()
    {
        character = GetComponent<Character>();
        character.ThirdPersonController.OnJump += JumpHandler;
    }
    
    // Animations to code 
    // Idle, CrouchSneakLeft, CrouchSneakRight, CrouchToStand, FallingIdle, FallToLand
    // Idle, Jump, Run, StandToCrouch, StumbleBackwards, StumbleToStand, SwitchCharacter
    // Walk

    private void Update()
    {
        if (character.Controller.isGrounded)
        {
            character.Animator.SetFloat(AirFloat, 0);
            if (character.Controller.velocity.magnitude > 0.001f)
            {
                character.Animator.SetBool(IdleBool, false);
                character.Animator.SetFloat(GroundFloat, character.Controller.velocity.magnitude);
                character.Animator.ResetTrigger(JumpTrigger);
            }

            if (character.Controller.velocity.magnitude < 0.001f)
            {
                character.Animator.SetFloat(GroundFloat, 0f);
            }
        }
        if (!character.Controller.isGrounded)
        {
            NotGrounded();
            character.Animator.SetFloat(AirFloat, Mathf.Abs(character.Controller.velocity.y) + 0.1f);
        }
    }
    
    private void JumpHandler()
    {
        character.Animator.SetTrigger(JumpTrigger);
    }
    
    private void NotGrounded()
    {
        character.Animator.SetFloat(GroundFloat, 0);
        character.Animator.SetBool(IdleBool, false);
    }
}