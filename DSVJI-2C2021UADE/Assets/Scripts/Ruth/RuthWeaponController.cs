using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuthWeaponController : MonoBehaviour
{
    [SerializeField] private GameObject _equippedPrimary;
    [SerializeField] private GameObject _equippedSecondary;

    [SerializeField] private GameObject _unequippedPrimary;
    [SerializeField] private GameObject _unequippedSecondary;
    
    private bool _drawn;

    public void DrawSaveWeapon()
    {
        _drawn = !_drawn;
        
        _equippedPrimary.SetActive(_drawn);
        _equippedSecondary.SetActive(_drawn);

        _unequippedPrimary.SetActive(!_drawn);
        _unequippedSecondary.SetActive(!_drawn);
        

    }
}
