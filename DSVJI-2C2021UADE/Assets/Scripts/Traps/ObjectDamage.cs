using System.Collections;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private float damage = 15f;
    [SerializeField] private float damageIntervalTimer = 1f;
#pragma warning restore 649

    #endregion

    private bool _hasCollided;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (!_hasCollided)
        {
            StartCoroutine(DamageInterval());
            print("I damaged");
            _hasCollided = true;
        }
        //other.gameObject.GetComponent<Health>().TakeDamage(damage);
    }

    private IEnumerator DamageInterval()
    {
        if (_hasCollided)
        {
            yield break;
        }
        yield return new WaitForSeconds(damageIntervalTimer);
        _hasCollided = false;
    }
}