using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private float damage;
#pragma warning restore 649
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Character>().Health.TakeDamage(damage);
        }
    }
}
