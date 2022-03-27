using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selector : BehaviourNode
{
    int lastProcessingIndex;
    List<BehaviourNode> children = new List<BehaviourNode>();

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var childTransform = transform.GetChild(i);
            var childNode = childTransform.GetComponent<BehaviourNode>();

            if (childNode != null)
                children.Add(childNode);
            else
                Debug.LogError("Missing Node in Sequence", childTransform.gameObject);
        }
    }
    // Me fijo si se puede ejecutar el nodo
    public override bool CanExecute(AIController ai)
    {
        for (int i = 0; i < children.Count; i++)
            if (children[i].CanExecute(ai))
                return true;

        return false;
    }

    protected override BehaviourResult ExecuteInternal(AIController ai)
    {   
        // Recorro todos los nodos hasta el anterior al que se estaba ejecutando
        // para verificar si se pueden ejecutar. Si alguno se puede ejecutar
        // cancelo la ejecución del que se estaba ejecutando (processing)
        for (int i = 0; i < lastProcessingIndex; i++)
        {
            var child = children[i];
            if (child.CanExecute(ai))
            {
                // Como se que voy a cambiar la ejecucion a otro nodo desde uno que estaba
                // processing, le digo a ese que estaba en processing que finalize
                children[lastProcessingIndex].End(ai);
                
                // Si uno de los de la izquierda del que es processing se puede ejecutar
                // le digo al Selector que a continuacion arranque ejecutando ese
                lastProcessingIndex = i;
                break;
            }
        }
        
        for (int i = lastProcessingIndex; i < children.Count; i++)
        {
            var child = children[i];
            var result = child.Execute(ai);

            if (result == BehaviourResult.Processing)
            {
                lastProcessingIndex = i;
                return BehaviourResult.Processing;
            }

            if (result == BehaviourResult.Success)
            {
                lastProcessingIndex = 0;
                return BehaviourResult.Success;
            }
        }

        lastProcessingIndex = 0;
        return BehaviourResult.Failure;
    }
    
    public override void End(AIController ai)
    {
        if (LastResult == BehaviourResult.Processing)
            children[lastProcessingIndex].End(ai);

        lastProcessingIndex = 0;
    }
}