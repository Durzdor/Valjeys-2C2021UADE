using System.Collections;
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
    
    private bool _isGasActive;

    private void Update()
    {
        StartCoroutine(ObjectGas());
    }
    
    private IEnumerator ObjectGas()
    {
        if (_isGasActive)
            yield break;
        _isGasActive = true;
        float counter = 0;
        
        yield return new WaitForSeconds(gasInterval);
        
        while (counter < gasDuration)
        {
            counter += Time.deltaTime;
            gasGameObject.SetActive(true);
            yield return null;
        }
        
        gasGameObject.SetActive(false);
        _isGasActive = false;
    }
}
