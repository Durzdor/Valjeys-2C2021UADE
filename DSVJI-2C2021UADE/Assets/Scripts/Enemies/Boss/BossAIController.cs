using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIController : MonoBehaviour
{

    #region Serializables

    [SerializeField]
    private List<BossAttack> _attacks;
    [SerializeField]
    private LayerMask _player;
    [SerializeField]
    [Range(1, 2)]
    private float _meleeAttackRange;
    [SerializeField]
    [Range(2, 4)]
    private float _spellAreaRange;
    [SerializeField]
    private Renderer _mat;

    #endregion

    #region Propiedades privadas

    private bool _stunned;
    private bool _canAttack;
    private bool _chasingPlayer;
    private bool _playerOnMeleeRange;
    private bool _playerOnAreaSpellRange;

    #endregion

    void Start()
    {
        _mat.material.color = Color.Lerp(Color.magenta, Color.black, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        RangeCheck();
        Action();
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

    #endregion
}
