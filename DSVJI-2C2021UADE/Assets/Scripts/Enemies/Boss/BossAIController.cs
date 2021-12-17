using Assets.Scripts.Enemies.Boss.Attacks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BossAIController : MonoBehaviour
{

    #region Serializables

    [SerializeField]
    private LayerMask _player;
    [SerializeField]
    [Range(3, 4)]
    private float _meleeAttackRange;
    [SerializeField]
    [Range(5, 8)]
    private float _spellAreaRange;
    [SerializeField]
    [Range(9, 10)]
    private float _proyectileRange;
    [SerializeField]
    private Renderer _mat;
    [SerializeField]
    private List<BaseAttack> _attacks;
    [SerializeField]
    [Range(1, 5)]
    private int _attackIntervalInSeconds;

    #endregion

    #region Propiedades privadas

    private bool _playerOnMeleeRange;
    private bool _playerOnAreaSpellRange;
    private bool _playerOnProyectileRange;

    private TimeSpan _ts;
    private Stopwatch _sw;
    private Blackboard _memory;

    #endregion

    void Start()
    {
        _mat.material.color = Color.Lerp(Color.magenta, Color.black, 0.5f);
        _ts = new TimeSpan(0, 0, _attackIntervalInSeconds);
        _sw = new Stopwatch();
        _sw.Start();
        _memory = GetComponent<Blackboard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_memory.Get("PlayerPosition") != null)
            transform.LookAt((Vector3)_memory.Get("PlayerPosition"));
        RangeCheck();
        Action();
    }

    #region Metodos Privados

    private void RangeCheck()
    {
        //Collider[] _meleeAttackArea = Physics.OverlapSphere(transform.position, _meleeAttackRange, _player);
        //if (_meleeAttackArea.Length > 0)
        //{
        //    _playerOnMeleeRange = true;
        //    _playerOnAreaSpellRange = false;
        //    _playerOnProyectileRange = false;
        //    _memory.Set("PlayerPosition", _meleeAttackArea[0].transform.position);
        //    return;
        //}

        Collider[] _areaSpells = Physics.OverlapSphere(transform.position, _spellAreaRange, _player);
        if (_areaSpells.Length > 0)
        {
            _playerOnMeleeRange = false;
            _playerOnAreaSpellRange = true;
            _playerOnProyectileRange = false;
            _memory.Set("PlayerPosition", _areaSpells[0].transform.position);
            return;
        }

        Collider[] _areaProyectile = Physics.OverlapSphere(transform.position, _proyectileRange, _player);
        print(_areaProyectile.Length);
        if (_areaProyectile.Length > 0)
        {
            _playerOnMeleeRange = false;
            _playerOnAreaSpellRange = false;
            _playerOnProyectileRange = true;
            _memory.Set("PlayerPosition", _areaProyectile[0].transform.position);
            return;
        }

    }

    private void Action()
    {
        if (_sw.Elapsed < _ts) return;
        
        if (_playerOnAreaSpellRange) _attacks[0].Attack();

        else if (_playerOnProyectileRange) _attacks[1].Attack();

        //else if (_playerOnMeleeRange) _attacks[2].Attack();

        _sw.Restart();
    }

    private void TestAttacks()
    {
        if (Input.GetKey(KeyCode.A))
            _attacks[0].Attack();
        if (Input.GetKey(KeyCode.B))
            _attacks[1].Attack();
        if (Input.GetKey(KeyCode.B))
            _attacks[2].Attack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _meleeAttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _spellAreaRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _proyectileRange);
    }

    #endregion
}
