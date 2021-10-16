using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSeePlayerNode : BehaviourNode
{
    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        if ((bool)ai.Memory.Get("onSightTarget"))
        {
            return BehaviourResult.Success;
        }
        return BehaviourResult.Failure;
    }
}
