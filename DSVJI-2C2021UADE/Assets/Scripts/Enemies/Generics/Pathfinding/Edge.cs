using System;
using UnityEngine;

[Serializable]
public class Edge
{
    //Levantar dentro de cada arista los nodos de ambos extremos, para
    //despues trazar el camino a seguir mas facilmente.
    [SerializeField] private float weight;
    [SerializeField] private Node target;
    public float Weight { get => weight; set => weight = value; }
    public Node Target { get => target; set => target = value; }
}
