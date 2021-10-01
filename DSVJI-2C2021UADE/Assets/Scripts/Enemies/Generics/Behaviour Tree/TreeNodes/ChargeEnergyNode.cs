using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnergyNode : BehaviourNode
{
    private float _actualEnergy;
    private float _maxEnergy;
    [SerializeField] private float _energyPerSeconds;
    private Color _lastColor;

    public override void Init(AIController ai)
    {
        //Obtengo la energia actual y maxima posible.
        _actualEnergy = (float) ai.Memory.Get("Energy");
        _maxEnergy = (float) ai.Memory.Get("MaxEnergy");
        _lastColor = ai.Body.GetComponent<MeshRenderer>().material.color;
        ai.Body.GetComponent<MeshRenderer>().material.color = Color.blue;
        ai.SteeringBehaviour.velocity = Vector3.zero;
    }

    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        Debug.Log("CargandoEnergia");
        
        // Recargo la energía
        _actualEnergy += _energyPerSeconds;

        // Si la energía se sigue recargando devuelvo que está en proceso
        // Sino devuelvo "Success" para que sepa que ya cargó toda la energía
        return _actualEnergy < _maxEnergy ? BehaviourResult.Processing : BehaviourResult.Success;
        
    }

    public override void End(AIController ai)
    {
        ai.Memory.Set("Energy", _actualEnergy);
        ai.Body.GetComponent<MeshRenderer>().material.color = _lastColor;
    }
}