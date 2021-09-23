using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTrap : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Rotation Trap Settings")] [Space(5)]
    [SerializeField] private bool canRotateBack = false;
    [SerializeField] private Vector3 targetRotation = new Vector3(0f,-90f,0f);
    [SerializeField] private float rotationDuration = 2f;
    [SerializeField] private float rotationBackInterval = 1f;
    [SerializeField] private float rotationStartInterval = 1f;
    [SerializeField] private Transform rotationPivot;
#pragma warning restore 649
    #endregion
    
    private bool isRotationActive = false;

    private void Update()
    { 
        StartCoroutine(ObjectRotation());
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
            yield return new WaitForSeconds(rotationBackInterval);

            while (counter > 0)
            {
                counter -= Time.deltaTime;
                LerpRot(defaultAngles, maxRotation, counter);
                yield return null;
            }
            
            yield return new WaitForSeconds(rotationStartInterval);
        }

        isRotationActive = false;
    }
    
    private void LerpRot(Vector3 defaultValue, Vector3 targetValue, float counter)
    {
        rotationPivot.eulerAngles = Vector3.Lerp(defaultValue, targetValue, counter / rotationDuration);
    }
}
