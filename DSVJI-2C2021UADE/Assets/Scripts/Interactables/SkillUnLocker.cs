using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnLocker : Interactable
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private bool unlockNaomiSkill;
    [SerializeField] private string skillName;
    [SerializeField] private int skillIndex;
#pragma warning restore 649

    #endregion

    private bool _isOpening;
    private bool _openComplete;
    private bool _skillAcquired;
    private float _waitInterval = 5.15f;
    private Animator _animator;
    private AudioSource _audioSource;
    private static readonly int OpenChest = Animator.StringToHash("OpenTrigger");
    
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
            StartCoroutine(ChestOpening());
        }
    }
    
    private IEnumerator ChestOpening()
    {
        if (_isOpening)
            yield break;
        _isOpening = true;
        _animator.SetTrigger(OpenChest);
        yield return new WaitForSeconds(_waitInterval);
        _animator.ResetTrigger(OpenChest);
        _openComplete = true;
        InteractableName = skillName;
        ResetLabel();
    }

    private void SkillAcquired()
    {
        if (_skillAcquired) return;
        if (_audioSource.clip == null) return;
        _audioSource.Play();
        _skillAcquired = true;
        InteractableName = "Skill Acquired";
        ResetLabel();
    }

    private void ResetLabel()
    {
        if (Character is null) return;
        Character.Interactable = this;
        Character.IsInInteractRange = false;
        Character.IsInInteractRange = true;
    }
}
