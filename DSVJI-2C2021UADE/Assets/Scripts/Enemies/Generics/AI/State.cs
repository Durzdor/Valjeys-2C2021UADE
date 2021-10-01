using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public string stateName;

    public virtual void InitState(AIController_FSM ai)
    {
    }

    public virtual void UpdateState(AIController_FSM ai)
    {
    }

    public virtual void EndState(AIController_FSM ai)
    {
    }
}
