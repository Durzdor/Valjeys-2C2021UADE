using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruth : MonoBehaviour
{

    private RuthWeaponController _ruthWeaponController;
    
    void Start()
    {
        _ruthWeaponController = GetComponent<RuthWeaponController>();
    }

    
    void Update()
    {
        if (Input.GetButtonDown("DrawSaveWeapon"))
        {
            _ruthWeaponController.DrawSaveWeapon();
        }
    }
}
