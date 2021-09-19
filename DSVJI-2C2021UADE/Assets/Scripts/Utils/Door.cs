using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private Vector3 targetRotation = new Vector3(0f,-90f,0f);
    [SerializeField] private float duration = 2f;
    [SerializeField] private float waitInterval = 1f;
    [SerializeField] private Transform doorPivot;
#pragma warning restore 649
    #endregion
    
    private bool isActive = false;
    private Transform playerTransform;
    private Vector3 currentTargetRotation;

    private void Start()
    {
        InteractableName = "Door";
    }

    private IEnumerator DoorRotation()
    {
        if (isActive)
            yield break;
        isActive = true;
        currentTargetRotation = targetRotation;
        if (!(Character is null)) playerTransform = Character.transform;
        float counter = 0;
        Vector3 defaultAngles = doorPivot.eulerAngles;

        if (PlayerIsBehindDoor())
            currentTargetRotation = -currentTargetRotation;

        Vector3 openRotation = transform.eulerAngles + currentTargetRotation;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            LerpDoor(defaultAngles, openRotation, counter);
            yield return null;
        }

        yield return new WaitForSeconds(waitInterval);

        while (counter > 0)
        {
            counter -= Time.deltaTime;
            LerpDoor(defaultAngles, openRotation, counter);
            yield return null;
        }

        isActive = false;
    }

    private void LerpDoor(Vector3 defaultAngles, Vector3 targetRotation, float counter)
    {
        doorPivot.eulerAngles = Vector3.Lerp(defaultAngles, targetRotation, counter / duration);
    }

    private bool PlayerIsBehindDoor()
    {
        Vector3 doorTransformDirection = transform.TransformDirection(Vector3.forward);
        Vector3 playerTransformDirection = playerTransform.position - transform.position;
        return Vector3.Dot(doorTransformDirection, playerTransformDirection) < 0;
    }

    public override void Interaction()
    {
        if (!(Character is null)) StartCoroutine(DoorRotation());
    }
}