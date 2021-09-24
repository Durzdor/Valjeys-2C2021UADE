using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : Interactable
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField][Range(3,30)] private float waitInterval = 4f;
#pragma warning restore 649
    #endregion
    
    private bool _isActive;
    private Transform _playerTransform;
    private Animator _animator;
    
    private static readonly int OpenPositiveZTrigger = Animator.StringToHash("OpenPositiveZ");
    private static readonly int OpenNegativeZTrigger = Animator.StringToHash("OpenNegativeZ");
    private static readonly int ClosePositiveZTrigger = Animator.StringToHash("ClosePositiveZ");
    private static readonly int CloseNegativeZTrigger = Animator.StringToHash("CloseNegativeZ");
    
    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        InteractableName = "Door";
    }
    
    private IEnumerator DoorRotation()
    {
        if (_isActive)
            yield break;
        _isActive = true;

        var openedFrom = PlayerIsBehindDoor() ? OpenPositiveZTrigger : OpenNegativeZTrigger;
        _animator.SetTrigger(openedFrom);
        yield return new WaitForSeconds(waitInterval);
        _animator.SetTrigger((openedFrom == OpenPositiveZTrigger) ? ClosePositiveZTrigger : CloseNegativeZTrigger);
       
        _isActive = false;
    }
    
    private bool PlayerIsBehindDoor()
    {
        var doorTransformDirection = transform.TransformDirection(Vector3.forward);
        var playerTransformDirection = _playerTransform.position - transform.position;
        return Vector3.Dot(doorTransformDirection, playerTransformDirection) < 0;
    }
    
    public override void Interaction()
    {
        if (Character is null) return;
        _playerTransform = Character.transform;
        StartCoroutine(DoorRotation());
    }
}
