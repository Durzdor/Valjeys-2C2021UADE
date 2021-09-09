using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject _equippedPrimary;
    [SerializeField] private GameObject _equippedSecondary;

    [SerializeField] private GameObject _unequippedPrimary;
    [SerializeField] private GameObject _unequippedSecondary;
    
    private bool _drawn;
    public delegate void AttackDelegate(Vector3 direction, Vector3 position);
    public AttackDelegate Attack;

    public void DrawSaveWeapon()
    {
        _drawn = !_drawn;

        if (_equippedPrimary && _unequippedPrimary)
        {
            _equippedPrimary.SetActive(_drawn);
            _unequippedPrimary.SetActive(!_drawn);
        }

        if (_equippedSecondary && _unequippedSecondary)
        {
            _equippedSecondary.SetActive(_drawn);
            _unequippedSecondary.SetActive(!_drawn);
        }
        

    }
}
