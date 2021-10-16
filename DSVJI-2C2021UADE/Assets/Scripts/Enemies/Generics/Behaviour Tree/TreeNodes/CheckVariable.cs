using System;
using UnityEngine;
using System.Collections;

public class CheckVariable : BehaviourNode
{
    [SerializeField] private string _variableToComparate;
    [SerializeField] private ComparationType _comparationType;
    [SerializeField] private float _floatToComparate;

    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        switch (_comparationType)
        {
            case ComparationType.LesserThan:
                return (float)ai.Memory.Get(_variableToComparate) < _floatToComparate ? BehaviourResult.Success : BehaviourResult.Failure;
                break;
            case ComparationType.GreaterThan:
                return (float)ai.Memory.Get(_variableToComparate) > _floatToComparate ? BehaviourResult.Success : BehaviourResult.Failure;
                break;
            case ComparationType.LesserEqualThan:
                return (float)ai.Memory.Get(_variableToComparate) <= _floatToComparate ? BehaviourResult.Success : BehaviourResult.Failure;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return BehaviourResult.Failure;
    }
}