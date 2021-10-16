using UnityEngine;
using System.Collections;


public class AIController_FSM : AIController
{
    
    [Header("Finite State Machine")]
    [SerializeField] private FiniteStateMachine fsm;
    public FiniteStateMachine Fsm{ get => fsm; set => fsm = value; }

    protected override void Awake()
    {
        base.Awake();
        fsm = GetComponent<FiniteStateMachine>();
    }
    
    private void Update()
    {
        print(fsm.currentState.stateName);
    }
}