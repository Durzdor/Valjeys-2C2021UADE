using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill Data", order = 51)]
public class SkillData : ScriptableObject
{
    [SerializeField] private float skillCooldown;
    [SerializeField] private float skillCost;

    public float SkillCooldown => skillCooldown;
    public float SkillCost => skillCost;
}
