using UnityEngine;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Enemies.DamagePopup
{ 
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField]
        private int _hp;
        [SerializeField]
        private GameObject _dmgPopup;

        [SerializeField] private Collider _collider;

        private Stopwatch _sw;
        private TimeSpan _ts;

        #region Unity

        void Start()
        {
            _sw = new Stopwatch();
            _ts = new TimeSpan(0, 0, 0, 1);
            _sw.Start();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerWeapon"))
            {
                if (_hp == 1)
                    Die();
                else
                {
                    var previousHp = _hp;
                    _hp--;
                    DamageDisplay(previousHp - _hp);
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
