using System;
using System.Collections;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private static readonly int VelX = Animator.StringToHash("VelX");
    private static readonly int VelZ = Animator.StringToHash("VelZ");
    private static readonly int Jump = Animator.StringToHash("JumpTrigger");
    private static readonly int Landing = Animator.StringToHash("LandingTrigger");
    private static readonly int Switch = Animator.StringToHash("SwitchTrigger");
    private static readonly int Death = Animator.StringToHash("DeathTrigger");
    private static readonly int Chest = Animator.StringToHash("ChestTrigger");
    private static readonly int Interact = Animator.StringToHash("InteractTrigger");
    private static readonly int Skill1Trigger = Animator.StringToHash("Skill1Trigger");
    private static readonly int Skill2Trigger = Animator.StringToHash("Skill2Trigger");
    private static readonly int Skill3Trigger = Animator.StringToHash("Skill3Trigger");
    private static readonly int Skill4Trigger = Animator.StringToHash("Skill4Trigger");
    private static readonly int Skill5Trigger = Animator.StringToHash("Skill5Trigger");

    public event Action OnSwitchComplete;
    public event Action OnDeathComplete;
    public event Action OnInteractionComplete;
    
    private Character _character;
    private int LocomotionLayer => _character.Animator.GetLayerIndex("Locomotion");
    private int SpecialActionsLayer => _character.Animator.GetLayerIndex("SpecialActions");
    private int NaomiLayer => _character.Animator.GetLayerIndex("Naomi");
    private int RuthLayer => _character.Animator.GetLayerIndex("Ruth");

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private void Start()
    {
        _character.CharacterMovement.OnJump += JumpHandler;
        _character.CharacterMovement.OnLanding += LandingHandler;
        _character.OnCharacterSwitch += SwitchHandler;
        _character.Health.OnDeath += DeathHandler;
        _character.OnCharacterInteract += InteractHandler;
        _character.SkillController.OnSkill1 += delegate { SkillHandler(Skill1Trigger); };
        _character.SkillController.OnSkill2 += delegate { SkillHandler(Skill2Trigger); };
        _character.SkillController.OnSkill3 += delegate { SkillHandler(Skill3Trigger); };
        _character.SkillController.OnSkill4 += delegate { SkillHandler(Skill4Trigger); };
        _character.SkillController.OnSkill5 += delegate { SkillHandler(Skill5Trigger); };
    }

    private void Update()
    {
        WalkAnimations();
        RunAnimations();
    }
    
    private void WalkAnimations()
    {
        if (!_character.CharacterMovement.IsGrounded) return;
        _character.Animator.SetFloat(VelX,_character.Input.HorizontalAxis);
        _character.Animator.SetFloat(VelZ,_character.Input.VerticalAxis);
    }

    private void RunAnimations()
    {
        if (!_character.CharacterMovement.IsSprinting) return;
        _character.Animator.SetFloat(VelX, _character.Animator.GetFloat(VelX) * 5);
        _character.Animator.SetFloat(VelZ, _character.Animator.GetFloat(VelZ) * 5);
    }
    
    private void JumpHandler()
    {
        _character.Animator.SetTrigger(Jump);
        _character.Animator.ResetTrigger(Landing);
    }

    private void LandingHandler()
    {
        _character.Animator.SetTrigger(Landing);
    }
    
    private void SwitchHandler()
    {
        ChangeLayerWeight(SpecialActionsLayer,1);
        _character.Animator.SetTrigger(Switch);
    }

    private void DeathHandler()
    {
        ChangeLayerWeight(SpecialActionsLayer,1);
        _character.Animator.SetTrigger(Death);
    }
    
    private void InteractHandler()
    {
        if (_character.Interactable is null || _character.Interactable.Name == "Skill Acquired") return;
        ChangeLayerWeight(SpecialActionsLayer,1);
        _character.IsAnimationLocked = true;
        if (_character.Interactable.Name == "Chest")
        {
            _character.Animator.SetTrigger(Chest);
        }
        else
        {
            _character.Animator.SetTrigger(Interact);
            OnInteractionComplete?.Invoke();
        }
    }
    
    private void SkillHandler(int id)
    {
        _character.IsAnimationLocked = true;
        var layer = _character.IsNaomi ? NaomiLayer : RuthLayer;
        ChangeLayerWeight(layer,1);
        _character.Animator.SetTrigger(id);
    }
    
    // Chest Animation event uses this method
    public void ChestAnimationOpenEvent()
    {
        _character.Animator.ResetTrigger(Chest);
        OnInteractionComplete?.Invoke();
    }
    
    // RDeath Animation event uses this method
    public void DeathAnimationEndedEvent()
    {
        _character.Animator.ResetTrigger(Death);
        OnDeathComplete?.Invoke();
        ChangeLayerWeight(SpecialActionsLayer,0);
    }

    public void SwitchAnimationEndedEvent()
    {
        _character.Animator.ResetTrigger(Switch);
        OnSwitchComplete?.Invoke();
        ChangeLayerWeight(SpecialActionsLayer,0);
    }
    
    // Event to unlock the animations
    public void AnimationUnLocker()
    {
        _character.IsAnimationLocked = false;
    }

    private void ChangeLayerWeight(int layer, float weight)
    {
        _character.Animator.SetLayerWeight(layer,weight);
    }

    // Special Layer Events
    public void RemoveLockWeightSpecial()
    {
        AnimationUnLocker();
        ChangeLayerWeight(SpecialActionsLayer,0);
    }

    // Naomi Layer Events
    public void RemoveLockWeightNaomi()
    {
        AnimationUnLocker();
        ChangeLayerWeight(NaomiLayer,0);
    }
    
    // Ruth Layer Events
    public void RemoveLockWeightRuth()
    {
        AnimationUnLocker();
        ChangeLayerWeight(RuthLayer,0);
    }
}
