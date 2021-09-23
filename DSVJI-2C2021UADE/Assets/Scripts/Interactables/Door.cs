using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : Interactable
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField][Range(3,6)] private float waitInterval = 4f;
#pragma warning restore 649
    #endregion
    
    private bool isActive = false;
    private Transform playerTransform;
    private Animator animator;
    
    private static readonly int OpenPositiveZTrigger = Animator.StringToHash("OpenPositiveZ");
    private static readonly int OpenNegativeZTrigger = Animator.StringToHash("OpenNegativeZ");
    private static readonly int ClosePositiveZTrigger = Animator.StringToHash("ClosePositiveZ");
    private static readonly int CloseNegativeZTrigger = Animator.StringToHash("CloseNegativeZ");
    
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        InteractableName = "Door";
    }
    
    private IEnumerator DoorRotation()
    {
        if (isActive)
            yield break;
        isActive = true;

        var openedFrom = PlayerIsBehindDoor() ? OpenPositiveZTrigger : OpenNegativeZTrigger;
        animator.SetTrigger(openedFrom);
        yield return new WaitForSeconds(waitInterval);
        animator.SetTrigger((openedFrom == OpenPositiveZTrigger) ? ClosePositiveZTrigger : CloseNegativeZTrigger);
       
        isActive = false;
    }
    
    private bool PlayerIsBehindDoor()
    {
        Vector3 doorTransformDirection = transform.TransformDirection(Vector3.forward);
        Vector3 playerTransformDirection = playerTransform.position - transform.position;
        return Vector3.Dot(doorTransformDirection, playerTransformDirection) < 0;
    }
    
    public override void Interaction()
    {
        if (Character is null) return;
        playerTransform = Character.transform;
        StartCoroutine(DoorRotation());
    }
}
