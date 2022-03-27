using UnityEngine;
using System.Diagnostics;
using System;
using Assets.Scripts.Enemies.DamagePopup;

public class BaseEnemy : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private GameObject dmgPopup;
    [SerializeField] private Transform dmgPopupPosition;
#pragma warning restore 649

    #endregion

    private Camera _characterCamera;
    private Animator _anim;
    public Health Health { get; private set; }
    private Stopwatch _sw;
    private TimeSpan _ts;
    
    private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");

    #region Unity

    // private void Start()
    // {
    //     _sw = new Stopwatch();
    //     _ts = new TimeSpan(0, 0, 0, 1);
    //     _sw.Start();
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if ((other.CompareTag("PlayerWeapon") || other.CompareTag("PlayerProjectile")) && _sw.Elapsed > _ts)
    //     {
    //         if (_hp == 1)
    //             Die();
    //         else
    //         {
    //             _hp--;
    //             _anim.SetTrigger("TakeDamage");
    //             DamageDisplay(1); // _damageHandler.TotalDamageTaken
    //             _sw.Restart();
    //         }
    //     }
    // }

    #endregion

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        Health = GetComponent<Health>();
    }

    private void Start()
    {
        _characterCamera = Camera.main;
        Health.OnConsumed += OnHealthConsumedHandler;
        Health.OnDeath += Die;
    }

    private void OnHealthConsumedHandler()
    {
        //_anim.SetTrigger(TakeDamage);
        DamageDisplay(Health.LastDamageTaken);
    }
    
    private void Die()
    {
        //TODO: Animacion de muerte.
        Destroy(gameObject);
    }

    private void DamageDisplay(float damage)
    {
        var popup = Instantiate(dmgPopup).GetComponent<DamagePopup>();
        var pos = dmgPopupPosition.position;
        // transform.position - (_characterCamera.ScreenToWorldPoint(_characterCamera.transform.position) - transform.position).normalized;
        popup.ShowDamage(pos, damage);
    }
}