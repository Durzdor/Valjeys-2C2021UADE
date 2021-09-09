using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruth : MonoBehaviour
{

    private WeaponController _ruthWeaponController;
    
    void Start()
    {
        _ruthWeaponController = GetComponent<WeaponController>();
    }

    
    void Update()
    {
        if (Input.GetButtonDown("DrawSaveWeapon"))
        {
            _ruthWeaponController.DrawSaveWeapon();
        }
    }
}
