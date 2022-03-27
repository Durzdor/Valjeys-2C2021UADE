using System;
using System.Collections.Generic;
using UnityEngine;


public class Ruth : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Switch")] [Space(5)]
    [SerializeField] private Material ruthMaterial;
#pragma warning restore 649
    #endregion

    public Material RuthMaterial => ruthMaterial;
    public WeaponController WeaponController { get; private set; }

    public event Action OnRuthEnable;
    
    private void OnEnable()
    {
        if (WeaponController) WeaponController.ActivateEquippedWeapons();
        OnRuthEnable?.Invoke();
    }

    private void Start()
    {
        WeaponController = GetComponent<WeaponController>();
    }

    
    private void Update()
    {
        if (Input.GetButtonDown("DrawSaveWeapon"))
        {
            WeaponController.DrawSaveWeapon();
        }
    }
}
