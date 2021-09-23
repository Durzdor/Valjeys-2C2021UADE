using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasTrap : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Gas Trap Settings")] [Space(5)]
    [SerializeField] private float gasDuration = 1f;
    [SerializeField] private float gasInterval = 1f;
    [SerializeField] private GameObject gasGameObject;
#pragma warning restore 649
    #endregion
    
    private bool isGasActive = false;

    private void Update()
    {
        StartCoroutine(ObjectGas());
    }
    
    private IEnumerator ObjectGas()
    {
        if (isGasActive)
            yield break;
        isGasActive = true;
        float counter = 0;
        
        yield return new WaitForSeconds(gasInterval);
        
        while (counter < gasDuration)
        {
            counter += Time.deltaTime;
            gasGameObject.SetActive(true);
            yield return null;
        }
        
        gasGameObject.SetActive(false);
        isGasActive = false;
    }
}
