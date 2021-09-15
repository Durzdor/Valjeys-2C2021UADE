
public class Checkpoint : Interactable
{
    private void Start()
    {
        InteractableName = "Checkpoint";
    }

    public override void Interaction()
    {
        if (Character == null) return;
        Character.SaveCheckpoint(transform);
    }
}
