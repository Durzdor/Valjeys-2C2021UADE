using UnityEngine;

public class OrbDoor : Door
{
    [SerializeField] private string doorName = "Door to altar";
    [SerializeField] private bool needCustomOrbAmount = false;
    [SerializeField] private int orbCustomAmount = 1;
    

    private int OrbsRequired => (needCustomOrbAmount ? orbCustomAmount : Character.OrbsNeeded) - Character.OrbsObtained;
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
