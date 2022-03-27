using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] List<Edge> childs;
    public bool closed = false;
    [SerializeField] private Node previousNode;
    public float HCost;
    public float GCost;
    public float FCost { get => GCost + HCost; }



    public List<Edge> Childs { get => childs; set => childs = value; }
    public Node PreviousNode { get => previousNode; set => previousNode = value; }
    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        for (int i = 0; i < childs.Count; i++)
        {
            if (childs[i].Target == null) continue;

            var diff = childs[i].Target.transform.position - transform.position;
            var arrowPos = transform.position + diff * 0.8f;

            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, childs[i].Target.transform.position);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(arrowPos, 0.25f);
        }
    }

    public List<Node> FindPath(Node destination)
    {
        //Esta lista almacena los nodos que queremos explorar
        var openSet = new List<Node>();

        //Me agrego a mi mismo para empezar la busqueda desde aca
        openSet.Add(this);

        //Recorro todos los objetos que quiero explorar
        for (int i = 0; i < openSet.Count; i++)
        {
            //Accedo al nodo que quiero explorar
            Node open = openSet[i];

            //Exploro el nodo verificando cada uno de los hijos del mismos
            for (int j = 0; j < open.childs.Count; j++)
            {
                //Accedo a una de las aristas del nodo a explorar (el abierto)
                Edge child = open.childs[j];

                //Primero verifico si el nodo al que se conecta la arista (target)
                //es el destino al que quiero llegar
                if (child.Target == destination)
                {
                    //Le digo al nodo hijo que su padre (open) fue el primero en visitarlo
                    child.Target.previousNode = open;

                    var actualNode = destination;
                    var pathFinded = new List<Node>();
                    

                    while (actualNode != this)
                    {
                        pathFinded.Add(actualNode);
                        actualNode = actualNode.previousNode;
                    }

                    pathFinded.Reverse();
                    print("Lo encontré: " + pathFinded);
                    return pathFinded;
                }

                //Verifico si este hijo del abierto no está en la lista de abierto, osea
                //verifico que no haya sido recorrido o que no se vaya a recorrer
                if (!openSet.Contains(child.Target))
                {
                    //Le digo al nodo hijo que su padre (open) fue el primero en visitarlo
                    child.Target.previousNode = open;

                    //Si este hijo del abierto no es el destino puede ser un nodo
                    //que me lleve al destino, por lo que lo agrego al openSet para
                    //explorarlo mas tarde
                    openSet.Add(child.Target);
                }
            }
        }
        return new List<Node>();
    }

    #region Dijkstra

    public float AccumulatedWeight = Mathf.Infinity;
    public List<Node> Dijkstra(Node destination)
    {
        //Creo la lista openSet
        List<Node> openSet = new List<Node>();
        //Como este es el nodo de inicio, su peso acumulado va a ser 0.
        AccumulatedWeight = 0;

        //Me agrego a openSet para explorarme.
        openSet.Add(this);

        //Iteramos la lista openSet
        for (int i = 0; i < openSet.Count; i++)
        {
            var currentNode = openSet[i];
            Debug.Log(currentNode.name);
            //Iteramos los hijos del nodo en el que estamos explorando
            for (int j = 0; j < currentNode.childs.Count; j++)
            {
                var target = currentNode.childs[j].Target;

                //Agrego el target si contiene un peso acumulado mayor al mio.
                if (target.AccumulatedWeight > currentNode.AccumulatedWeight + currentNode.childs[j].Weight)
                {
                    //Seteo al padre como el previousNode.
                    target.previousNode = openSet[i];
                    //Calculo su peso acumulado.
                    target.AccumulatedWeight = currentNode.AccumulatedWeight + currentNode.childs[j].Weight;
                    //Agrego a mi hijo a openSet
                    openSet.Add(target);
                }
            }
        }

        var actualNode = destination;
        var pathFinded = new List<Node>();

        while (actualNode != this)
        {
            pathFinded.Add(actualNode);
            actualNode = actualNode.previousNode;
        }

        //Seteo los pesos acumulados de todos los nodos en infinito para una posible nueva busqueda.
        foreach (var item in openSet)
        {
            item.AccumulatedWeight = Mathf.Infinity;
        }

        pathFinded.Reverse();
        print("Lo encontré: " + pathFinded);
        return pathFinded;
    }

    #endregion

    #region Astar
    public List<Node> Astar(Node target)
    {
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(this);

        while (openSet.Count > 0)
        {
            Node node = openSet[0];

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == target)
            {
                List<Node> pathFinded = new List<Node>();
                Node currentNode = target;

                while (currentNode != this)
                {
                    pathFinded.Add(currentNode);
                    currentNode = currentNode.PreviousNode;
                }
                pathFinded.Add(this);
                pathFinded.Reverse();
                return pathFinded;
            }
            
            foreach (var child in node.Childs)
            {
                Node neighbour = child.Target;
                if (!closedSet.Contains(neighbour))
                {
                    float newCostChild = node.GCost + child.Weight;
                    if (newCostChild < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newCostChild;
                        neighbour.HCost = GetDistance(neighbour, target);
                        neighbour.PreviousNode = node;
                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        return openSet;
    }

    private float GetDistance(Node node1, Node node2)
    {
        float xDist = Mathf.Abs(node1.transform.position.x - node2.transform.position.x);
        float zDist = Mathf.Abs(node1.transform.position.z - node2.transform.position.z);

        return xDist > zDist ? zDist + (xDist - zDist) : xDist + (zDist - xDist);

        //return Vector3.Distance(node1.gameObject.transform.position, node2.gameObject.transform.position);
    }

    public static List<Node> LazyTheta(List<Node> path)
    {
        List<Node> lazyNodes = new List<Node>();
        Node currentNode = path[0];


        while (true)
        {
            if (currentNode == path[path.Count - 1])
                break;

            for (int i = path.Count - 1; i >= 0; i--)
            {
                var dir = Vector3.Normalize(path[i].transform.position - currentNode.transform.position);
                if (!Physics.Raycast(currentNode.transform.position, dir, LayerMask.NameToLayer("Obstacle")))
                {
                    lazyNodes.Add(path[i]);
                    currentNode = path[i];
                    break;
                }
            }
        }

        return lazyNodes;
    }

    #endregion

    //Verifico si aun quedan nodos por visitar.
    public bool AllNodesClosed(List<Node> openSet)
    {
        foreach (var item in openSet)
        {
            if (!item.closed)
                return false;
        }
        return true;
    }
}