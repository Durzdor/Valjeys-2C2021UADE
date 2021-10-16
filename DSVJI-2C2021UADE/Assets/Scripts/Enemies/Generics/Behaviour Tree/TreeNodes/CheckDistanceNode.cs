using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum ComparationType
{
    LesserThan, GreaterThan, LesserEqualThan
}

public class CheckDistanceNode : BehaviourNode
{
    [SerializeField] private ComparationType _comparationType;
    [SerializeField] private float _distanceToCheck;
    [SerializeField] private Transform _transformToComparate;

    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        var dist = 0f;
        switch (_comparationType)
        {
            case ComparationType.LesserThan:
                dist = Vector3.Distance(((GameObject) ai.Memory.Get("body")).transform.position, _transformToComparate.position);
                return dist < _distanceToCheck ? BehaviourResult.Success : BehaviourResult.Failure;
            case ComparationType.GreaterThan:
                dist = Vector3.Distance(((GameObject) ai.Memory.Get("body")).transform.position,_transformToComparate.position);
                return dist > _distanceToCheck ? BehaviourResult.Success : BehaviourResult.Failure;
            default:
                Debug.LogError("Error with comparation type.");
                break;
        }

        return BehaviourResult.Failure;
    }
}