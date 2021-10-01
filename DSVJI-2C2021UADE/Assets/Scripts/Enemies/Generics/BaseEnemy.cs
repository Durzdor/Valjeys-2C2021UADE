using UnityEngine;
using System.Diagnostics;
using System;
using Assets.Scripts.Enemies.DamagePopup;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    private int _hp;
    [SerializeField]
    private GameObject _dmgPopup;
    [SerializeField]
    private TakeDamageHandler _damageHandler;
    [SerializeField]
    private Animator _anim;

    private Stopwatch _sw;
    private TimeSpan _ts;

    #region Unity

    private void Start()
    {
        _sw = new Stopwatch();
        _ts = new TimeSpan(0, 0, 0, 1);
        _sw.Start();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("PlayerWeapon") || other.CompareTag("PlayerProjectile")) && _sw.Elapsed > _ts)
        {
            if (_hp == 1)
                Die();
            else
            {
                _hp--;
                _anim.SetTrigger("TakeDamage");
                DamageDisplay(_damageHandler.TotalDamageTaken);
                _sw.Restart();
            }
        }
    }

    #endregion

        private void Die()
        {
            //TODO: Animacion de muerte.
            Destroy(gameObject);
        }

        private void DamageDisplay(int damage)
        {
            DamagePopup dmgPopup = Instantiate(_dmgPopup).GetComponent<DamagePopup>();
            Vector3 pos = transform.position - (Camera.main.ScreenToWorldPoint(Camera.main.transform.position) - transform.position).normalized;
            pos.y = transform.position.y * 2;
            dmgPopup.ShowDamage(pos, damage);
        }
    }
