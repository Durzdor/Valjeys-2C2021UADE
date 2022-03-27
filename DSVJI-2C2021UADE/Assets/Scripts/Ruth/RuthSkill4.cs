using UnityEngine;

public class RuthSkill4 : Skill
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Skill Specific")][Space(5)]
    [SerializeField] private SkillData data;
#pragma warning restore 649
    #endregion
    
    private void OnEnable()
    {
        Data = data;
    }
    
    public override void UseSkill()
    {
        base.UseSkill();
        if (CanUseSkill)
        {
            SkillAction();
        }
    }

    private void SkillAction()
    {
        print($"I used {name}!");
    }
}
