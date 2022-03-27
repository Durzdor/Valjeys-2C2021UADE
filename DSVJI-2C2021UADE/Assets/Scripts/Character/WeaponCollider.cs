using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public bool onAttack { get; private set; }

    public bool striked { get; private set; }

    private float _weaponDamage;

    public void OnAttack(float attackDuration)
    {
        onAttack = true;
        Invoke(nameof(DeactivateAttack), attackDuration);
    }

    public void Striked()
    {
        striked = true;
    }
    
    private void DeactivateAttack()
    {
        onAttack = false;
        striked = false;
        UpdateWeaponDamage(0f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        var enemyHealth = other.GetComponent<BaseEnemy>();
        if (enemyHealth)
        {
            enemyHealth.Health.TakeDamage(_weaponDamage);
        }
    }

    public void UpdateWeaponDamage(float damage)
    {
        _weaponDamage = damage;
    }
}