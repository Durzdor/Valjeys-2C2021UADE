using UnityEngine;
using System.Collections.Generic;


public class WanderNode : BehaviourNode
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private float length = 3f;
    [SerializeField] private float randomRangeAngle = 3f;
    [SerializeField] private LayerMask whatIsObstacle;
#pragma warning restore 649

    #endregion

    private Collider[] detectedColliders;
    
    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        
        ai.SteeringBehaviour.Wander(length, randomRangeAngle, ai.MaxSpeed, ai.SteeringSpeed);

        // Flee obstacles
        // Me fijo todos los objetos con layer Obstacle
        var closesObjects = Physics.OverlapSphere(ai.Body.transform.position, 3f, whatIsObstacle);
        if (closesObjects.Length > 0)
        {
            // Los recorro y comparo para ver cual es el más cercano
            var closeObject = closesObjects[0];
            foreach (var obstacle in closesObjects)
            {
                if (Vector3.Distance(obstacle.transform.position, ai.Body.transform.position) < Vector3.Distance(closeObject.transform.position, ai.Body.transform.position))
                    closeObject = obstacle;
            }
            // Me alejo del más cercano
            ai.SteeringBehaviour.Flee(closeObject.transform.position, ai.MaxSpeed * 2, ai.SteeringSpeed * 2);
            print("fleewander");
        }
        
        return BehaviourResult.Success;
    }
}