using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : BehaviourNode
{
    [SerializeField] private Character _player;
    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        throw new System.NotImplementedException("Invocar metodo de daño al personaje.");
        //print("attack");
        //return BehaviourResult.Success;
    }
}
