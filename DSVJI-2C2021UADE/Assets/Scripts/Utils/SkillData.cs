using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill Data", order = 51)]
public class SkillData : ScriptableObject
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Skill properties")] [Space(5)]
    [SerializeField] private float cooldown;
    [SerializeField] private float damage;
    [SerializeField] private float useCost;
    [SerializeField] private Sprite image;
    
#pragma warning restore 649
    #endregion

    public float Cooldown => cooldown;
    public float UseCost => useCost;
    public float Damage => damage;
    public Sprite Image => image;
}
