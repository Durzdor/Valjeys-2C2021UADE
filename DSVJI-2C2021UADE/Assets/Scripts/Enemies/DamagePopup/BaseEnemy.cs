using UnityEngine;
using System.Diagnostics;
using System;

namespace Assets.Scripts.Enemies.DamagePopup
{ 
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField]
        private int _hp;
        [SerializeField]
        private GameObject _dmgPopup;

        private Stopwatch _sw;
        private TimeSpan _ts;

        #region Unity

        void Start()
        {
            _sw = new Stopwatch();
            _ts = new TimeSpan(0, 0, 0, 1);
            _sw.Start();
        }

        void Update()
        {
            if (_sw.Elapsed > _ts && Input.GetKey(KeyCode.A))
            {
                DamageDisplay();
                _sw.Restart();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("PlayerWeapon"))
            {
                if (_hp == 1)
                    Die();
                else
                { 
                    _hp--;
                    DamageDisplay();
                }
            }
        }

        #endregion

        private void Die()
        { 
        
        }

        private void DamageDisplay()
        {
            DamagePopup dmgPopup = Instantiate(_dmgPopup).GetComponent<DamagePopup>();
            Vector3 pos = transform.position - (Camera.main.ScreenToWorldPoint(Camera.main.transform.position) - transform.position).normalized;
            pos.y = transform.position.y * 2;
            dmgPopup.ShowDamage(pos, 100);
        }
    }
}
