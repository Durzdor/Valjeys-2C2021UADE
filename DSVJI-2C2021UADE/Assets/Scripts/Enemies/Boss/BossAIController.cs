using Assets.Scripts.Enemies.Boss.Attacks;
using System.Collections.Generic;
using UnityEngine;

public class BossAIController : MonoBehaviour
{

    #region Serializables

    [SerializeField]
    private LayerMask _player;
    [SerializeField]
    [Range(1, 2)]
    private float _meleeAttackRange;
    [SerializeField]
    [Range(2, 4)]
    private float _spellAreaRange;
    [SerializeField]
    [Range(4, 10)]
    private float _proyectileRange;
    [SerializeField]
    private Renderer _mat;
    [SerializeField]
    private List<BaseAttack> _attacks;

    #endregion

    #region Propiedades privadas

    private bool _stunned;
    private bool _canAttack;
    private bool _chasingPlayer;
    private bool _playerOnMeleeRange;
    private bool _playerOnAreaSpellRange;

    private Blackboard _memory;

    #endregion

    void Start()
    {
        _mat.material.color = Color.Lerp(Color.magenta, Color.black, 0.5f);
        _memory = GetComponent<Blackboard>();
    }

    // Update is called once per frame
    void Update()
    {
        RangeCheck();

        
    }

    #region Metodos Privados

    private void RangeCheck()
    {
        Collider[] _meleeAttackArea = Physics.OverlapSphere(transform.position, _meleeAttackRange, _player);
        if (_meleeAttackArea.Length > 0)
        {
            _playerOnMeleeRange = true;
            return;
        }

        Collider[] _areaSpells = Physics.OverlapSphere(transform.position, _spellAreaRange, _player);
        if (_areaSpells.Length > 0)
        {
            _playerOnAreaSpellRange = true;
            return;
        }

        Collider[] _areaProyectile = Physics.OverlapSphere(transform.position, _proyectileRange, _player);
        print(_areaProyectile.Length);
        if (_areaProyectile.Length > 0)
        {
            print("Player!!");
            _memory.Set("PlayerPosition", _areaProyectile[0].transform.position);
            return;
        }

    }

    private void Action()
    {
        if (_playerOnAreaSpellRange)
        {

        }

        else if (_playerOnMeleeRange)
        {

        }

        else
        { 
            //TODO: Deberia o hacer un ataque a distancia o acercarse al player para golpearlo a melee/area.
        }

    }

    private void TestAttacks()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _attacks[0].Attack();
        }
        if (Input.GetKey(KeyCode.B))
        {
            _attacks[1].Attack();
        }
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
