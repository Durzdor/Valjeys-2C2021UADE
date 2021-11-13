using UnityEngine;

public class AIController : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private Transform target;
    [SerializeField] private GameObject body;
    [SerializeField] private Blackboard memory;
    [SerializeField] private Sensor sensor;
    [SerializeField] private SteeringBehaviour steeringBehaviour;
#pragma warning restore 649

    #endregion
    
    private float energy;
    [SerializeField] private float maxEnergy = 1f;

    [Header("Movement")] [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float steeringSpeed = 1f;

    public GameObject Body{ get => body; set => body = value; }
    public Blackboard Memory{ get => memory; set => memory = value; }
    
    public Sensor Sensor{ get => sensor; set => sensor = value; }
    public SteeringBehaviour SteeringBehaviour{ get => steeringBehaviour; set => steeringBehaviour = value; }
    public float MaxSpeed{ get => maxSpeed; set => maxSpeed = value; }
    public float SteeringSpeed{ get => steeringSpeed; set => steeringSpeed = value; }

    protected virtual void Awake()
    {
        memory = GetComponent<Blackboard>();
        sensor = GetComponent<Sensor>();
        memory.Set("body", body);
        memory.Set("target", target);
        memory.Set("MaxEnergy", maxEnergy);
        memory.Set("Energy", maxEnergy);
    }
}