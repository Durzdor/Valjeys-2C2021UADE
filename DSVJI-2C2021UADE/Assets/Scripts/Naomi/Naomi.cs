 using System;
 using System.Collections.Generic;
 using UnityEngine;

public class Naomi : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Switch")] [Space(5)]
    [SerializeField] private Material naomiMaterial;
    [Header("FireBall")] [Space(5)]
    [SerializeField] private Projectile projectileGameObject;
    [SerializeField] private Transform projectileSpawnPoint;
    [Range(0, 1)][SerializeField] private float animationSyncTime;
    [Range(0, 5)] [SerializeField] private float animationEndTime;
    [Range(0, 10)][SerializeField] private float attackCooldown;
#pragma warning restore 649
    #endregion
    
    public Material NaomiMaterial => naomiMaterial;
    public WeaponController WeaponController { get; private set; }

    private float _attackCooldownTimer;
    // private bool _onAnimation;
    
    public event Action OnNaomiEnable;
    
    private void OnEnable()
    {
        if (!WeaponController) Start();
        WeaponController.DeactivateWeapons();
        OnNaomiEnable?.Invoke();
    }
    
    void Start()
    {
        WeaponController = GetComponent<WeaponController>();
        WeaponController.Attack = Attack;
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
        // _onAnimation = false;
    }

    void Update()
    {

        if (_attackCooldownTimer > 0) _attackCooldownTimer -= Time.deltaTime; 
        
        if (Input.GetButtonDown("Fire1") && _attackCooldownTimer <= 0)
        {
            _attackCooldownTimer = attackCooldown + 0.1f;
            Invoke(nameof(Attack), animationSyncTime);
            
            Invoke(nameof(EndAnimation), animationEndTime);
        }
    }
}
