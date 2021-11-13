using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForAttackNode : BehaviourNode
{
    [SerializeField] private int timeToListened = 0;
    [Range(0f, 10f)] [SerializeField] private float distance = 0;
    private float timeToWait;
    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        // Verifico si el tiempo ya pasó y devuelvo falso
        if (timeToWait > timeToListened)
        {
            timeToWait = 0;
            return BehaviourResult.Failure;
        }
        else
        {
            // Si no pasó el tiempo de espera me fijo si el target esta dentro de la distancia
            if (Vector3.Distance(((Transform)ai.Memory.Get("target")).position, ai.Body.transform.position) < distance)
            {
                // Si lo está devuelvo success
                timeToWait = 0;
                return BehaviourResult.Success;
            }
            // Mientras que no esté y el tiempo siga siendo menor, le sumo al tiempo.
            timeToWait += Time.deltaTime;
            return BehaviourResult.Processing;
        }
    }
}
