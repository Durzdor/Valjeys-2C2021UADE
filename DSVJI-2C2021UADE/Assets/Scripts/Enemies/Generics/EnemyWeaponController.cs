using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [SerializeField]
    private int _damage = 1;
    [SerializeField]
    private int _attackTimer;
    private Stopwatch _sw;
    private TimeSpan _ts;

    private void Awake()
    {
        _sw = new Stopwatch();
        _sw.Start();
        _ts = new TimeSpan(0, 0, _attackTimer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _sw.Elapsed >= _ts)
        { 
            other.GetComponent<Health>().TakeDamage(_damage);
            _sw.Restart();
        }
    }
}
