using System;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] private AIController aiController;
    [SerializeField] private float repeatingTime;
    [SerializeField] private LayerMask whatIsObstacle;
    [SerializeField] private LayerMask sound;
    [SerializeField] private float coneAngle = 0.7f;
    [SerializeField] private float circleOverlapRadius;

    private Vector3 bodyPosition;
    static Collider[] detectedColliders = new Collider[100];

    private void Awake()
    {
        aiController = GetComponent<AIController>();
        InvokeRepeating("SensorAction", 0f, 0.0001f);
    }

    private void Start()
    {
        bodyPosition = ((GameObject) aiController.Memory.Get("body")).transform.position;
    }

    private void SensorAction()
    {
        aiController.Memory.Set("onSightTarget", CanSeeTarget());
        aiController.Memory.Set("rayToTarget", RayTarget());
        if (aiController.Memory.Get("nodeTarget") != null)
            aiController.Memory.Set("nodeReached", TargetReached());
        aiController.Memory.Set("canHearPlayer", CanHearPlayer());
    }

    private bool CanHearPlayerBetweenWalls()
    {
        //Corroborar la potencia esperada para determinar si el enemigo me puede escuchar.
        float potenciaEsperada = 10f;
        float potencia = (float)aiController.Memory.Get("potenciaRuidoJugador");
        float cantidadPuertas = 0f;
        float radio = (float)aiController.Memory.Get("radioRuidoJugador");
        //Posicion del jugador
        Vector3 playerPosition = (Vector3)aiController.Memory.Get("playerPosition");
        //Disparo un raycast hacia el jugador para ver la cantidad de puertas que hay en el medio.
        RaycastHit[] hits = Physics.RaycastAll(aiController.transform.position, playerPosition);
        //Por cada collider que sea pared, sumo 1 a cantidadParedes
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.name.Contains("Wall"))
            {
                cantidadPuertas++;
            }
        }
        
        //Formula de la intensidad del sonido.
        float intensidadSonido = potencia / (4f * (float)Math.PI * (float)Math.Pow(radio, 2));
        //Se eleva a 2 la cantidad de puertas para que en caso de que no haya paredes, para que no explote si se divide por 0.
        float reduccionPorObstaculos = 1 / (float)Math.Pow(2, cantidadPuertas);
        //Se multiplica para reducir la potencia del sonido si se cruza una pared.
        float intensidadRecibida = intensidadSonido * reduccionPorObstaculos;

        return intensidadRecibida >= potenciaEsperada;
    }

    private bool CanHearPlayer()
    {
        var coliders = Physics.OverlapSphere(aiController.Body.transform.position, circleOverlapRadius, sound);
        if (coliders.Length > 0)
        {
            aiController.Memory.Set("listened", coliders[0].transform.position);
            return true;
        }
        return false;
    }

    private bool TargetReached()
    {
        var distanceToTarget = Vector3.Distance(((GameObject) aiController.Memory.Get("body")).transform.position, ((Node) aiController.Memory.Get("nodeTarget")).transform.position);
        return distanceToTarget < 1f;
    }

    private bool CanSeeTarget()
    {
        int detectedAmount = Physics.OverlapSphereNonAlloc(transform.position, circleOverlapRadius, detectedColliders);

        for (int i = 0; i < detectedAmount; i++)
        {
            
            if (detectedColliders[i].gameObject == ((Transform)aiController.Memory.Get("target")).gameObject)
            {
                Vector3 lookDir = transform.forward;
                Vector3 targetDir = Vector3.Normalize(detectedColliders[i].transform.position - transform.position);

                float dot = Vector3.Dot(lookDir, targetDir);

                if (dot >= coneAngle)
                {
                    var targetPosition = detectedColliders[i].transform.position;
                    Vector3 dirToTarget = Vector3.Normalize(targetPosition - bodyPosition);
                    float distToTarget = Vector3.Distance(targetPosition, bodyPosition);
                    bool hit = Physics.Raycast(bodyPosition, dirToTarget, distToTarget, whatIsObstacle);
                    //print("Visto");
                    return !hit;
                }
            }
        }

        return false;
    }

    private bool RayTarget()
    {
        var targetPosition = ((Transform) aiController.Memory.Get("target")).position;
        Vector3 dirToTarget = Vector3.Normalize(targetPosition - bodyPosition);
        float distToTarget = Vector3.Distance(targetPosition, bodyPosition);
        bool hit = Physics.Raycast(bodyPosition, dirToTarget, distToTarget, whatIsObstacle);
        return !hit;
    }

/*
    public bool DetectObstacles(out GameObject closeObj)
    {
        var position = aiController.Body.transform.position;
        var dir = aiController.Body.transform.forward;
        var dist = 2f;

        var detectedAmountColliders = Physics.OverlapSphereNonAlloc(position, dist, detectedColliders, whatIsObstacle);
        if (detectedAmountColliders > 0)
        {
            closeObj = detectedColliders[0].gameObject;
            for (int i = 0; i < detectedAmountColliders; i++)
            {
                if (Vector3.Distance(detectedColliders[i].transform.position, position) < Vector3.Distance(closeObj.transform.position, position))
                {
                    closeObj = detectedColliders[i].gameObject;
                }
            }

            return true;
        }

        closeObj = null;
        return false;
    }
    */

    private void OnDrawGizmos()
    {
        // OverlapSphere
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, circleOverlapRadius);
        
    }
}