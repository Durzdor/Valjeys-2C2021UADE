using UnityEngine;

public class OrbDoor : Door
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private string doorName = "Door to altar";
    [SerializeField] private bool needCustomOrbAmount;
    [SerializeField] private int orbCustomAmount = 1;
#pragma warning restore 649
    #endregion
    
    private int OrbsRequired
    {
        get
        {
            if (!(Character is null))
                return (needCustomOrbAmount ? orbCustomAmount : Character.OrbsNeeded) - Character.OrbsObtained;
            return 0;
        }
    }

    protected override void Start()
    {
        base.Start();
        InteractableName = doorName;
    }
    public override void Interaction()
    {
        if (OrbsRequired <= 0)
        {
            base.Interaction();
        }
        else
        {
            if (!(Character is null))
                Character.NotificationPopup.SetMessage(
                    "Door is Locked"
                    , $"To unlock this door collect the remaining {OrbsRequired} orb");
        }
    }
}
