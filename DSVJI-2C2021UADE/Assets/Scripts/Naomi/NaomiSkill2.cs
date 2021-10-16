using UnityEngine;

public class NaomiSkill2 : Skill
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private SkillData data;
    [Header("FireBall")] [Space(5)]
    [SerializeField] private FireWall fireWallGameObject;
    [SerializeField] private Transform fireWallSpawnPoint;
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
        var direction = transform.forward;
        var position = fireWallSpawnPoint.position;
        
        var fireWall = Instantiate(fireWallGameObject, position, transform.rotation);
        fireWall.Init(position);
    }
}
