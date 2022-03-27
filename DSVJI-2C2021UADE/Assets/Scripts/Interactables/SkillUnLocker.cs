using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillUnLocker : Interactable
{
    #region SerializedFields

#pragma warning disable 649
    [Header("Skill")][Space(5)]
    [SerializeField] private bool unlockNaomiSkill;
    [SerializeField] private string skillName;
    [SerializeField] private int skillIndex;
#pragma warning restore 649

    #endregion

    private bool _isOpening;
    private bool _openComplete;
    private bool _skillAcquired;
    private Animator _animator;
    private AudioSource _audioSource;
    
    private static readonly int OpenChest = Animator.StringToHash("OpenTrigger");
    
    public bool UnlockNaomiSkill => unlockNaomiSkill;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        InteractableName = "Chest";
    }
    
    public override void Interaction()
    {
        if (_openComplete)
        {
            if (Character is null) return;
            if (unlockNaomiSkill)
            {
                Character.SkillController.UnlockNaomiSkill(skillIndex);
            }
            else
            {
                Character.SkillController.UnlockRuthSkill(skillIndex);
            }
            SkillAcquired();
        }
        else
        {
            OpenChestLid();
        }
    }
    
    private void SkillAcquired()
    {
        if (_skillAcquired) return;
        if (_audioSource.clip == null) return;
        _audioSource.Play();
        _skillAcquired = true;
        ResetLabel();
    }

    private void ResetLabel()
    {
        if (Character is null) return;
        Character.Interactable = this;
        Character.IsInInteractRange = false;
        Character.IsInInteractRange = true;
    }

    private void OpenChestLid()
    {
        if (_isOpening) return;
        _isOpening = true;
        _animator.SetTrigger(OpenChest);
    }

    // Animation Event for completing the opening of the chest
    public void OpeningComplete()
    {
        _openComplete = true;
        InteractableName = skillName;
        ResetLabel();
    }
}
