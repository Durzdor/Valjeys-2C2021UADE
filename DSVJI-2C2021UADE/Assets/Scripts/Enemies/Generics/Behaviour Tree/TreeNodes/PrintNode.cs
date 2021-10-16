using UnityEngine;

public class PrintNode : BehaviourNode
{
    [SerializeField] private string _text;
    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        print(_text);
        return BehaviourResult.Success;
    }
}