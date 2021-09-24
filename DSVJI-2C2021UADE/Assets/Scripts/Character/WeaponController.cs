using UnityEngine;

public class WeaponController : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private GameObject _equippedPrimary;
    [SerializeField] private GameObject _equippedSecondary;

    [SerializeField] private GameObject _unequippedPrimary;
    [SerializeField] private GameObject _unequippedSecondary;
#pragma warning restore 649
    #endregion

    public bool drawn { get; private set; }

    public delegate void AttackDelegate();
    public AttackDelegate Attack;

    public void DrawSaveWeapon()
    {
        drawn = !drawn;

        if (_equippedPrimary && _unequippedPrimary)
        {
            _equippedPrimary.SetActive(drawn);
            _unequippedPrimary.SetActive(!drawn);
        }

        if (_equippedSecondary && _unequippedSecondary)
        {
            _equippedSecondary.SetActive(drawn);
            _unequippedSecondary.SetActive(!drawn);
        }
    }

    public void ActivateEquippedWeapons()
    {
        if (_equippedPrimary && _unequippedPrimary)
        {
            _equippedPrimary.SetActive(true);
            _unequippedPrimary.SetActive(false);
               
        }

        if (_equippedSecondary && _unequippedSecondary)
        {
            _equippedSecondary.SetActive(true);
            _unequippedSecondary.SetActive(false);
        }
    }

    public void DeactivateWeapons()
    {
        if (_equippedPrimary && _unequippedPrimary)
        {
            _equippedPrimary.SetActive(false);
            _unequippedPrimary.SetActive(false);
                
        }

        if (_equippedSecondary && _unequippedSecondary)
        {
            _equippedSecondary.SetActive(false);
            _unequippedSecondary.SetActive(false);    
        }
    }
}
