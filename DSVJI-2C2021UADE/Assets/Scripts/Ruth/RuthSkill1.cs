using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuthSkill1 : Skill
{
    [SerializeField] private Character _character;
    [SerializeField] private WeaponCollider _weaponCollider;
    
    
    private void Awake()
    {
        _character = GetComponent<Character>();
    }
    
    private void Start()
    {
        _character.CharacterSkillController.Skill1 += ActivateWeapon;
        if (_weaponCollider) _weaponCollider.OnWeaponCollision += OnAttackCollision;
    }

    void ActivateWeapon()
    {
        
    }

    private void OnAttackCollision(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Play particles?
        }
    }

}
