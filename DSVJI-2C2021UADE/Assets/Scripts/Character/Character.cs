using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Health healthController;
    private Armor armorController;

    private void Awake()
    {
        healthController = GetComponent<Health>();
        armorController = GetComponent<Armor>();
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
