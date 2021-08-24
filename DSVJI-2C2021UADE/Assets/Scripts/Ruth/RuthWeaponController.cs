using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuthWeaponController : MonoBehaviour
{
    
    [SerializeField] private GameObject _rightHand;
    [SerializeField] private GameObject _leftHand;
    
    [SerializeField] private GameObject _primaryRestPosition;
    [SerializeField] private GameObject _secondaryRestPosition;

    [SerializeField] private GameObject _primaryWeapon;
    [SerializeField] private GameObject _secondaryWeapon;
    
    private bool _drawn;

    public void DrawSaveWeapon()
    {
        _primaryWeapon.transform.position = _drawn ? _primaryRestPosition.transform.position : _rightHand.transform.position;
        _primaryWeapon.transform.rotation = _drawn ? _primaryRestPosition.transform.rotation : _rightHand.transform.rotation;
        _secondaryWeapon.transform.position = _drawn ?  _secondaryRestPosition.transform.position : _leftHand.transform.position;
        _secondaryWeapon.transform.rotation = _drawn ?  _secondaryRestPosition.transform.rotation : _leftHand.transform.rotation;

        _drawn = !_drawn;
    }
}
