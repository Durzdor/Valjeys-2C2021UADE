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

    private bool _isOpen;
    private int _waitInterval = 1;
    private Animator _animator;
    private static readonly int OpenChest = Animator.StringToHash("OpenTrigger");
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        InteractableName = skillName;
    }
    
    public override void Interaction()
    {
        if (_isOpen) return;
        if (Character is null) return;
        if (unlockNaomiSkill)
            Character.SkillController.UnlockNaomiSkill(skillIndex);
        else
            Character.SkillController.UnlockRuthSkill(skillIndex);
        StartCoroutine(ChestOpening());
    }
    
    private IEnumerator ChestOpening()
    {
        if (_isOpen)
            yield break;
        _isOpen = true;
        _animator.SetTrigger(OpenChest);
        yield return new WaitForSeconds(_waitInterval);
        _animator.ResetTrigger(OpenChest);
    }
}
