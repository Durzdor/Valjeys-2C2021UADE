using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Health healthController;
    private Armor armorController;
    private Animator animator;
    private CharacterController characterController;
    private ThirdPersonController thirdPersonController;

    public Animator Animator => animator;
    public CharacterController Controller => characterController;
    public ThirdPersonController ThirdPersonController => thirdPersonController;

    private void Awake()
    {
        healthController = GetComponent<Health>();
        armorController = GetComponent<Armor>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        
        healthController.OnDamaged += OnHealthDamageTaken;
        armorController.OnDamaged += OnArmorDamageTaken;
    }

    private void OnHealthDamageTaken()
    {
        print($"Vida Actual: {healthController.CurrentHealth}/{healthController.MaxHealth}");
    }
    private void OnArmorDamageTaken()
    {
        print($"Vida Actual: {armorController.CurrentArmor}/{armorController.MaxArmor}");
    }
}
