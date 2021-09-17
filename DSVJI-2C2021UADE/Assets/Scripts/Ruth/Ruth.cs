using System;
using System.Collections.Generic;
using UnityEngine;


public class Ruth : MonoBehaviour
{
    
    private WeaponController _ruthWeaponController;

    #region SerializedFields
#pragma warning disable 649
    [Header("Switch")] [Space(5)]
    [SerializeField] private Material ruthMaterial;
    [SerializeField] private List<Sprite> ruthSkillImages;
    [SerializeField] private List<SkillData> ruthSkillData;
    [SerializeField] private Character _character;
    [SerializeField] private WeaponCollider _weaponCollider;
    [Range(0,2)][SerializeField] private float _animationDuration;
#pragma warning restore 649
    #endregion

    public Material RuthMaterial => ruthMaterial;
    public List<Sprite> RuthSkillImages => ruthSkillImages;
    public List<SkillData> RuthSkillData => ruthSkillData;

    public event Action OnRuthEnable;
    private void OnEnable()
    {
        // if (!_ruthWeaponController) Start();
        if (_ruthWeaponController) _ruthWeaponController.ActivateEquippedWeapons();
        OnRuthEnable?.Invoke();
    }

    void Start()
    {
        _ruthWeaponController = GetComponent<WeaponController>();
        _character.CharacterSkillController.Skill1 += OnSkillAttack;
    }

    
    void Update()
    {
        if (Input.GetButtonDown("DrawSaveWeapon"))
        {
            _ruthWeaponController.DrawSaveWeapon();
        }
    }

    void OnSkillAttack()
    {
        if (!_character.IsNaomi)
        {
            if (!_ruthWeaponController.drawn) _ruthWeaponController.DrawSaveWeapon();
            _weaponCollider.OnAttack(_animationDuration);    
        }
    }
    
}
