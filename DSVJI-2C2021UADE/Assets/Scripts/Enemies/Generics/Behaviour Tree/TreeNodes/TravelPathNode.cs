using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelPathNode : BehaviourNode
{
    private int index = 0;
    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        Node enemyClosestNode = (Node)ai.Memory.Get("enemyClosestNode");
        Node target = (Node)ai.Memory.Get("targetNode");
        List<Node> path = enemyClosestNode.Astar(target);
        if ((bool)ai.Memory.Get("nodeReached"))
        {
            return BehaviourResult.Success;
        }
        var actualNode = path[index];
        ai.SteeringBehaviour.Seek(actualNode.transform.position, ai.MaxSpeed, ai.SteeringSpeed);
        if (Vector3.Distance(transform.position, actualNode.transform.position) < 1.5f)
        {
            index ++;
        }
        return BehaviourResult.Processing;
    }

    
}
