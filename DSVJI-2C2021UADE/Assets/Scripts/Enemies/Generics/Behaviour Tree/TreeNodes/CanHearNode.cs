using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanHearNode : BehaviourNode
{
    
    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        if ((bool)ai.Memory.Get("canHearPlayer"))
        {
            return BehaviourResult.Success;
        }
        return BehaviourResult.Failure;
    }

}
