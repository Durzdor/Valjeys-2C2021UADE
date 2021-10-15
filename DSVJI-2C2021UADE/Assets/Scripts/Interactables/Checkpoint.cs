
using UnityEngine;

public class Checkpoint : Interactable
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private string checkpointName;
    
#pragma warning restore 649

    #endregion
    private void Start()
    {
        InteractableName = checkpointName;
    }

    public override void Interaction()
    {
        if (!(Character is null)) Character.SaveCheckpoint(transform);
    }
}
