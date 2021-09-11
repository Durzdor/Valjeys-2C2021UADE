using System;
using System.Collections.Generic;
using UnityEngine;


public class Ruth : MonoBehaviour
{
    private WeaponController _ruthWeaponController;
    
    [Header("Switch")] [Space(5)]
    [SerializeField] private Material ruthMaterial;
    [SerializeField] private List<Sprite> ruthSkillImages;
    [SerializeField] private List<SkillData> ruthSkillData;

    public Material RuthMaterial => ruthMaterial;
    public List<Sprite> RuthSkillImages => ruthSkillImages;
    public List<SkillData> RuthSkillData => ruthSkillData;

    public event Action OnRuthEnable;
    private void OnEnable()
    {
        OnRuthEnable?.Invoke();
    }
    
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
