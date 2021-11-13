using System;
using UnityEngine;
using System.Collections;

public class CheckVariable : BehaviourNode
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private string _variableToComparate;
    [SerializeField] private ComparationType _comparationType;
    [SerializeField] private float _floatToComparate;
#pragma warning restore 649

    #endregion

    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        switch (_comparationType)
        {
            case ComparationType.LesserThan:
                return (float)ai.Memory.Get(_variableToComparate) < _floatToComparate ? BehaviourResult.Success : BehaviourResult.Failure;
            case ComparationType.GreaterThan:
                return (float)ai.Memory.Get(_variableToComparate) > _floatToComparate ? BehaviourResult.Success : BehaviourResult.Failure;
            case ComparationType.LesserEqualThan:
                return (float)ai.Memory.Get(_variableToComparate) <= _floatToComparate ? BehaviourResult.Success : BehaviourResult.Failure;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}