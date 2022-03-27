using UnityEngine;

public class DummyNode : BehaviourNode
{
    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        print("Dummy!");

        return BehaviourResult.Success;
    }
}
