using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitNode : BehaviourNode
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private float secondsToPredict;
    [SerializeField] private float energyReduction;
#pragma warning restore 649

    #endregion

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