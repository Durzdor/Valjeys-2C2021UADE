 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naomi : MonoBehaviour
{
    [SerializeField] private Projectile projectileGameObject;
    [SerializeField] private Transform projectileSpawnPoint;
    
    private WeaponController _naomiWeaponController;
    
    void Start()
    {
        _naomiWeaponController = GetComponent<WeaponController>();
        _naomiWeaponController.Attack = Attack;
    }

    void Attack(Vector3 direction, Vector3 position)
    {
        var projectile = Instantiate(projectileGameObject, projectileSpawnPoint.position, transform.rotation);
        projectile.Init(direction, position);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _naomiWeaponController.Attack(transform.forward, projectileSpawnPoint.position);
        }
        
        if (Input.GetButtonDown("DrawSaveWeapon"))
        {
            _naomiWeaponController.DrawSaveWeapon();
        }
    }
}
