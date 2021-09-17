using System;
using System.Collections.Specialized;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public bool onAttack { get; private set; }

    public bool striked { get; private set; }

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
    }
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        
    }
}