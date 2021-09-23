using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTrap : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Shooter Trap Settings")] [Space(5)]
    [SerializeField] private bool canShootInBursts = false;
    [SerializeField] private float shootInterval = 1f;
    [SerializeField] private float burstObjectAmount = 3f;
    [SerializeField] private float burstShootInterval = 0.3f;
    [SerializeField] private List<Transform> shooterTransform;
    [SerializeField] private GameObject objectToShoot;
#pragma warning restore 649
    #endregion
    
    private bool isShooterActive = false;
    private int listPosition = 0;

    private void Update()
    {
        StartCoroutine(ObjectShooter());
    }
    
    private IEnumerator ObjectShooter()
    {
        if (isShooterActive)
            yield break;
        isShooterActive = true;
        var counter = 0;

        if (canShootInBursts)
        {
            while (counter < burstObjectAmount)
            {
                
                for (int i = 0; i < shooterTransform.Count; i++)
                {
                    if (counter < burstObjectAmount)
                    {
                        counter++;
                    }
                    else
                    {
                        break;
                    }
                    yield return new WaitForSeconds(burstShootInterval);
                    if (i >= shooterTransform.Count)
                    {
                        i = 0;
                    }
                    ShootObject(objectToShoot, shooterTransform[i]);
                }
                yield return null;
            }
        }
        else
        {
            ShootObject(objectToShoot, shooterTransform[listPosition]);
            listPosition++;
            if (listPosition >= shooterTransform.Count)
            {
                listPosition = 0;
            }
        }

        yield return new WaitForSeconds(shootInterval);
        isShooterActive = false;
    }
    
    private void ShootObject(GameObject shotObject, Transform shootLocation)
    {
        Instantiate(shotObject, shootLocation);
    }
}
