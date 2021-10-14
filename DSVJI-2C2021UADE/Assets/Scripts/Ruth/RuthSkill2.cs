using UnityEngine;

public class RuthSkill2 : Skill
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private SkillData data;
    [SerializeField] private WeaponCollider weaponCollider;
    [SerializeField] private float animationDuration;
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
        if (!Character.Ruth.WeaponController.drawn) Character.Ruth.WeaponController.DrawSaveWeapon();
        weaponCollider.OnAttack(animationDuration); 
    }
}
