using UnityEngine;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Enemies.DamagePopup
{ 
    public class BaseEnemy : MonoBehaviour
    {
        #region SerializedFields
#pragma warning disable 649
        [SerializeField]
        private int _hp;
        [SerializeField]
        private GameObject _dmgPopup;

        [SerializeField] private Collider _collider;
#pragma warning restore 649
        #endregion

        private Stopwatch _sw;
        private TimeSpan _ts;

        #region Unity

        void Start()
        {
            _sw = new Stopwatch();
            _ts = new TimeSpan(0, 0, 0, 1);
            _sw.Start();
        }

        private void TakeDamage(int damage = 1)
        {
            if (_hp == 1)
                Die();
            else
            {
                var previousHp = _hp;
                _hp-=damage;
                DamageDisplay(previousHp - _hp);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerProjectile"))
            {
                TakeDamage();
            }
            
            if (other.CompareTag("PlayerWeapon"))
            {
                var weaponCollider = other.GetComponent<WeaponCollider>();
                if (weaponCollider && weaponCollider.onAttack && !weaponCollider.striked)
                {
                    TakeDamage();
                    weaponCollider.Striked();
                }
            }
        }

        #endregion

        private void Die()
        { 
        
        }

        private void DamageDisplay(int damage)
        {
            DamagePopup dmgPopup = Instantiate(_dmgPopup).GetComponent<DamagePopup>();
            Vector3 pos = transform.position - (Camera.main.ScreenToWorldPoint(Camera.main.transform.position) - transform.position).normalized;
            pos.y = _collider.bounds.extents.y * 2.5f;
            dmgPopup.ShowDamage(pos, damage);
        }
    }
}
