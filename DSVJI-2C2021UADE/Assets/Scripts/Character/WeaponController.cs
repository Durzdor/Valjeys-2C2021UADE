using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private GameObject _equippedPrimary;
    [SerializeField] private GameObject _equippedSecondary;

    [SerializeField] private GameObject _unequippedPrimary;
    [SerializeField] private GameObject _unequippedSecondary;
#pragma warning restore 649
    #endregion

    public bool drawn { get; private set; }

    public delegate void AttackDelegate();
    public AttackDelegate Attack;

    public void DrawSaveWeapon()
    {
        drawn = !drawn;

        if (_equippedPrimary && _unequippedPrimary)
        {
            _equippedPrimary.SetActive(drawn);
            _unequippedPrimary.SetActive(!drawn);
        }

        if (_equippedSecondary && _unequippedSecondary)
        {
            _equippedSecondary.SetActive(drawn);
            _unequippedSecondary.SetActive(!drawn);
        }
        

    }
}
