using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAnimation : MonoBehaviour
{
    private Character character;

    private static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");
    private static readonly int LandTrigger = Animator.StringToHash("LandingTrigger");
    private static readonly int GroundFloat = Animator.StringToHash("GroundFloat");
    private static readonly int AirFloat = Animator.StringToHash("AirFloat");
    private static readonly int IdleBool = Animator.StringToHash("Idling");

    private void Start()
    {
        character = GetComponent<Character>();
        character.ThirdPersonController.OnJump += JumpHandler;
    }

    private void Update()
    {
        if (character.Controller.isGrounded)
        {
            Landing();
            // walk animation update
            if (character.Controller.velocity.magnitude > 0.001f)
            {
                character.Animator.SetBool(IdleBool, false);
                character.Animator.SetFloat(GroundFloat, character.Controller.velocity.magnitude);
                character.Animator.ResetTrigger(JumpTrigger);
            }

            // if not moving idle
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
        character.Animator.ResetTrigger(LandTrigger);
    }

    private void NotGrounded()
    {
        character.Animator.SetFloat(GroundFloat, 0);
        character.Animator.SetBool(IdleBool, false);
    }

    private void Landing()
    {
        if (character.Animator.GetFloat(AirFloat) > 0)
        {
            character.Animator.SetTrigger(LandTrigger);
        }

        character.Animator.SetFloat(AirFloat, 0);
    }
}