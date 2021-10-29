using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDoor : Door
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private string doorName = "Skill door";
    [SerializeField] private string skillName = "Skill name";
    [SerializeField] private bool naomiSkill;
    [SerializeField] private int skillIndex = 0;
#pragma warning restore 649
    #endregion
    
    private bool HasSkillRequired
    {
        get
        {
            if (Character is null) return false;
            var unlockedList = naomiSkill
                ? Character.SkillController.NaomiUnlockedSkillList
                : Character.SkillController.RuthUnlockedSkillList;
            return unlockedList[skillIndex];
        }
    }

    protected override void Start()
    {
        base.Start();
        InteractableName = doorName;
    }
    public override void Interaction()
    {
        if (HasSkillRequired)
        {
            base.Interaction();
        }
        else
        {
            if (!(Character is null))
                Character.NotificationPopup.SetMessage(
                    "Door is Locked"
                    , $"To unlock this door collect the Skill {skillName}");
        }
    }
}
