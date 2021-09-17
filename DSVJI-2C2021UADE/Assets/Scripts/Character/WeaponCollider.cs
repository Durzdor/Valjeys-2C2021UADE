using System;
using System.Collections.Specialized;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public delegate void WeaponCollision(Collider other);

    public event WeaponCollision OnWeaponCollision;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnWeaponCollision?.Invoke(other);
    }
}