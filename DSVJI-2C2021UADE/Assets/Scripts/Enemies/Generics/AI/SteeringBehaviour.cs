using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviour : MonoBehaviour
{
    AIController aiController;
    
    public Vector3 velocity;
    private Rigidbody rb;

    void Awake()
    {
        aiController = GetComponent<AIController>();
        rb = aiController.Body.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
        rb.velocity += velocity * Time.fixedDeltaTime;
        
        
        var faceDir = new Vector3(velocity.x, 0f, velocity.z);
            if (velocity.x != 0 && velocity.y != 0)
        {
            ((GameObject)aiController.Memory.Get("body")).transform.forward = faceDir;
        }
    }

    public void Seek(Vector3 targetPosition, float maxSpeed, float steeringSpeed)
    {
        
        Vector3 diff = targetPosition - aiController.Body.transform.position;
        Vector3 dir = Vector3.Normalize(diff);
        Vector3 desiredVelocity = dir * maxSpeed;
        
        Vector3 velDiff = desiredVelocity - velocity;
        Vector3 velDir = Vector3.Normalize(velDiff);
        Vector3 steeringForce = velDir * steeringSpeed;
        
        velocity += steeringForce * Time.deltaTime;
    }
    
    public void Flee(Vector3 targetPosition, float maxSpeed, float steeringSpeed)
    {
        Vector3 diff = aiController.Body.transform.position - targetPosition;
        Vector3 dir = Vector3.Normalize(diff);
        Vector3 desiredVelocity = dir * maxSpeed;

        Vector3 velDiff = desiredVelocity - velocity;
        Vector3 velDir = Vector3.Normalize(velDiff);
        Vector3 steeringForce = velDir * steeringSpeed;

        velocity += steeringForce * Time.deltaTime;
    }

    public void Arrival(Vector3 targetPosition, float radius, float maxSpeed, float steeringSpeed)
    {
        Vector3 bodyPos = aiController.Body.transform.position;
        float dist = Vector3.Distance(bodyPos, targetPosition);
        float distCoef = Mathf.Clamp(dist / radius, 0, 1);
        
        Vector3 diff = targetPosition - bodyPos;
        Vector3 dir = Vector3.Normalize(diff);
        Vector3 desiredVelocity = distCoef * maxSpeed * dir;
        
        Vector3 velDiff = desiredVelocity - velocity;
        Vector3 velDir = Vector3.Normalize(velDiff);
        Vector3 steeringForce = velDir * steeringSpeed;
        
        velocity += steeringForce * Time.deltaTime;
    }

    public void Pursuit(Vector3 targetPos, Vector3 targetVel, float secondsToPredict, float maxSpeed, float steeringSpeed)
    {
        Vector3 futurePos = targetPos + targetVel * secondsToPredict;
        Seek(futurePos, maxSpeed, steeringSpeed);
    }

    public void Wander(float length, float newAngle, float maxSpeed, float steeringSpeed)
    {
        Vector3 bodyPos = aiController.Body.transform.position;
        var newPos = bodyPos + Vector3.Normalize(velocity) * length;
        var displacement = new Vector3(Mathf.Cos(newAngle), 0f,Mathf.Sin(newAngle));
        var newVector = newPos + displacement;
        Seek(newVector, maxSpeed, steeringSpeed);
    }

    private List<Node> Pathfinding(Node start, Node end, List<Node> nodos)
    {
        List<Node> path = new List<Node>();
        path.Add(start);
        path.AddRange(start.FindPath(end));
        return path;
    }

    private void OnDrawGizmos()
    {
        // Velocidad actual
        Gizmos.color = Color.blue;
    }
}