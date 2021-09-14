using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill Data", order = 51)]
public class SkillData : ScriptableObject
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Skill properties")] [Space(5)]
    [SerializeField] private float skillCooldown;
    [SerializeField] private float skillCost;
#pragma warning restore 649
    #endregion

    public float SkillCooldown => skillCooldown;
    public float SkillCost => skillCost;
}
