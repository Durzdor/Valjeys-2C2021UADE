using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Rotation Trap Settings")] [Space(5)] 
    [SerializeField] private bool isRotationTrap = false;
    [SerializeField] private bool canRotateBack = false;
    [SerializeField] private Vector3 targetRotation = new Vector3(0f,-90f,0f);
    [SerializeField] private float rotationDuration = 2f;
    [SerializeField] private float rotationWaitInterval = 1f;
    [SerializeField] private Transform rotationPivot;
    [Header("Movement Trap Settings")] [Space(5)] 
    [SerializeField] private bool isMovementTrap = false;
    [SerializeField] private bool canMoveBack = false;
    [SerializeField] private Vector3 targetPosition = new Vector3(0f,-90f,0f);
    [SerializeField] private float movementDuration = 2f;
    [SerializeField] private float movementWaitInterval = 1f;
    [SerializeField] private Transform movementPivot;
    [Header("Triggerable Trap Settings")] [Space(5)] 
    [SerializeField] private bool isTriggerableTrap = false;
    [SerializeField] private GameObject triggerContainer;
#pragma warning restore 649
    #endregion
    
    private bool isRotationActive = false;
    private bool isMovementActive = false;
    private bool wasTriggered = false;
    
    private void Update()
    {
        if (isTriggerableTrap && wasTriggered)
        {
            
        }
        if (isRotationTrap)
        {
            StartCoroutine(ObjectRotation());
        }

        if (isMovementTrap)
        {
            StartCoroutine(ObjectMovement());
        }
    }

    private IEnumerator ObjectRotation()
    {
        if (isRotationActive)
            yield break;
        isRotationActive = true;
        float counter = 0;
        Vector3 defaultAngles = rotationPivot.eulerAngles;
        Vector3 maxRotation = transform.eulerAngles + targetRotation;

        while (counter < rotationDuration)
        {
            counter += Time.deltaTime;
            LerpRot(defaultAngles, maxRotation, counter);
            yield return null;
        }

        if (canRotateBack)
        {
            yield return new WaitForSeconds(rotationWaitInterval);

            while (counter > 0)
            {
                counter -= Time.deltaTime;
                LerpRot(defaultAngles, maxRotation, counter);
                yield return null;
            }
        }

        isRotationActive = false;
    }
    
    private IEnumerator ObjectMovement()
    {
        if (isMovementActive)
            yield break;
        isMovementActive = true;
        float counter = 0;
        Vector3 defaultPosition = movementPivot.position;
        Vector3 maxPosition = transform.position + targetPosition;

        while (counter < movementDuration)
        {
            counter += Time.deltaTime;
            LerpPos(defaultPosition, maxPosition, counter);
            yield return null;
        }

        if (canMoveBack)
        {
            yield return new WaitForSeconds(movementWaitInterval);

            while (counter > 0)
            {
                counter -= Time.deltaTime;
                LerpPos(defaultPosition, maxPosition, counter);
                yield return null;
            }
        }

        isMovementActive = false;
    }

    private void LerpRot(Vector3 defaultValue, Vector3 targetValue, float counter)
    {
        rotationPivot.eulerAngles = Vector3.Lerp(defaultValue, targetValue, counter / rotationDuration);
    }

    private void LerpPos(Vector3 defaultValue, Vector3 targetValue, float counter)
    {
        movementPivot.position = Vector3.Lerp(defaultValue, targetValue, counter / movementDuration);
    }

}
