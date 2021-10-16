using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sequence : BehaviourNode
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

    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        for (int i = lastProcessingIndex; i < children.Count; i++)
        {
            var child = children[i];
            var result = child.Execute(ai);

            if (result == BehaviourResult.Processing)
            {
                lastProcessingIndex = i;
                return BehaviourResult.Processing;
            }

            if (result == BehaviourResult.Failure)
            {
                lastProcessingIndex = 0;
                return BehaviourResult.Failure;
            }
        }

        lastProcessingIndex = 0;
        return BehaviourResult.Success;
    }

    public override void End(AIController ai)
    {
        if (LastResult == BehaviourResult.Processing)
            children[lastProcessingIndex].End(ai);
        
        lastProcessingIndex = 0;
    }
}