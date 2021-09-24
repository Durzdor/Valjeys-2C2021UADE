using UnityEngine;

public class NaomiSkill1 : Skill
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private SkillData data;
    [Header("FireBall")] [Space(5)]
    [SerializeField] private Projectile projectileGameObject;
    [SerializeField] private GameObject fireballParticles;
    [SerializeField] private Transform projectileSpawnPoint;
#pragma warning restore 649
    #endregion
    
    private void Start()
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
        var direction = transform.forward;
        var position = projectileSpawnPoint.position;
        
        var projectile = Instantiate(projectileGameObject, position, transform.rotation);
        Instantiate(fireballParticles, projectile.transform);
        projectile.Init(direction, position);
    }
}
