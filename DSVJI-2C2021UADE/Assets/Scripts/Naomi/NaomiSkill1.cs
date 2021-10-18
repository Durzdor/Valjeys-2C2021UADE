using UnityEngine;

public class NaomiSkill1 : Skill
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Skill Specific")][Space(5)]
    [SerializeField] private SkillData data;
    [SerializeField] private Projectile projectileGameObject;
    [SerializeField] private Transform projectileSpawnPoint;
#pragma warning restore 649
    #endregion
    
    private void OnEnable()
    {
        Data = data;
        projectileGameObject._powerLevel = 1;
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
        var direction = transform.forward;
        var position = projectileSpawnPoint.position;
        
        var projectile = Instantiate(projectileGameObject, position, transform.rotation);
        projectile.Init(direction, position);
    }
}
