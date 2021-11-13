using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoLastPositionNode : BehaviourNode
{
    // [SerializeField] private Node _endNode;

    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private List<Node> nodes;
    [SerializeField]private List<Node> path;
    [SerializeField] private int index;
    [SerializeField] private Node actualNode;
#pragma warning restore 649

    #endregion
    
    private Node closestNodeToListened;
    private Node closestNodeToMe;
    public override void Init(AIController ai)
    {
        var target = (Vector3)ai.Memory.Get("listened");
        closestNodeToListened = GetClosestNodeToPosition(target);
        closestNodeToMe = GetClosestNodeToPosition(ai.Body.transform.position);
        path = closestNodeToMe.Astar(closestNodeToListened);
        ai.Memory.Set("lastListenedPos", target);

        //ai.Memory.Set("nodeTarget", closestNodeToListened);
        //ai.Memory.Set("enemyClosestNode", closestNodeToMe);
    }

    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        // Me fijo si la distancia al nodo que quiero llegar es menor a 1.5f
        if (Vector3.Distance(closestNodeToListened.transform.position, ai.Body.transform.position) < 1.5f)
        {
            index = 0;
            ai.SteeringBehaviour.velocity = Vector3.zero;
            
            return BehaviourResult.Success;
        }

        // Logica de pathfinding para recorrer cada nodo hasta el destino
        actualNode = path[index];
        ai.SteeringBehaviour.Arrival(actualNode.transform.position, 0.5f, ai.MaxSpeed, ai.SteeringSpeed);
        if (Vector3.Distance(transform.position, actualNode.transform.position) < 1.5f)
        {
            index++;
        }            
        return BehaviourResult.Processing;
    }
    private Node GetClosestNodeToPosition(Vector3 position)
    {
        Node closestNode = nodes[0];

        foreach (var node in nodes)
        {
            var dist1 = Vector3.Distance(position, closestNode.transform.position);
            var dist2 = Vector3.Distance(position, node.transform.position);
            if (dist1 > dist2)
            {
                closestNode = node;
            }
        }
        return closestNode;
    }
}
