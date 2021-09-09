 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naomi : MonoBehaviour
{
    [SerializeField] private Projectile projectileGameObject;
    [SerializeField] private Transform projectileSpawnPoint;

    [Range(0, 1)][SerializeField] private float animationSyncTime;
    [Range(0, 5)] [SerializeField] private float animationEndTime;
    [Range(0, 10)][SerializeField] private float attackCooldown;

    private WeaponController _naomiWeaponController;
    private float _attackCooldownTimer;
    private bool _onAnimation;
    
    void Start()
    {
        _naomiWeaponController = GetComponent<WeaponController>();
        _naomiWeaponController.Attack = Attack;
    }

    void Attack()
    {
        var direction = transform.forward;
        var position = projectileSpawnPoint.position;
        
        var projectile = Instantiate(projectileGameObject, position, transform.rotation);
        projectile.Init(direction, position);
    }

    void EndAnimation()
    {
        _onAnimation = false;
    }

    void Update()
    {

        if (_attackCooldownTimer > 0) _attackCooldownTimer -= Time.deltaTime; 
        
        if (Input.GetButtonDown("Fire1") && _attackCooldownTimer <= 0)
        {
            if (!_naomiWeaponController.drawn) _naomiWeaponController.DrawSaveWeapon();
            
            _attackCooldownTimer = attackCooldown;
            Invoke(nameof(Attack), animationSyncTime);
            
            Invoke(nameof(EndAnimation), animationEndTime);
            
            // Play animation
        }
        
        if (Input.GetButtonDown("DrawSaveWeapon"))
        {
            _naomiWeaponController.DrawSaveWeapon();
        }
    }
}
