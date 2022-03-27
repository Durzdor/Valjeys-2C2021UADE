using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToListenedPositionNode : BehaviourNode
{
    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        // Si la ultima posicion escuchada es mayor a 1.5 a mi distancia me acerco a ella
        // si no la es, entonces me quedo quieto y devuelvo success
        if (Vector3.Distance((Vector3)ai.Memory.Get("lastListenedPos"), ai.Body.transform.position) > 1.5f)
        {
            ai.SteeringBehaviour.Arrival((Vector3)ai.Memory.Get("lastListenedPos"), 1.5f, ai.MaxSpeed, ai.SteeringSpeed);
        }
        else
        {
            ai.SteeringBehaviour.velocity = Vector3.zero;
            return BehaviourResult.Success;
        }
        return BehaviourResult.Processing;
    }
}
