using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitNode : BehaviourNode
{
    [SerializeField] private float secondsToPredict;
    
    
    [SerializeField] private float energyReduction;

    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        var target = (Transform) ai.Memory.Get("target");
        ai.SteeringBehaviour.Pursuit(target.position, target.GetComponent<Rigidbody>().velocity, secondsToPredict, ai.MaxSpeed, ai.SteeringSpeed);
        // Seteo la energía a su energía menos la energyReduction.
        var newEnergy = (float) ai.Memory.Get("Energy") - energyReduction;
        ai.Memory.Set("Energy", newEnergy);
        return BehaviourResult.Success;
    }
}