using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    AIController_FSM aiController;

    public List<State> states;
    public State currentState;

    //Esto es una función de inicialización que se ejecuta una sola vez
    void Awake()
    {
        //Con GetComponent le pedimos a Unity que busque el componente AIController
        //en el objeto en el cual estamos ahora
        aiController = GetComponent<AIController_FSM>();

        if (currentState == null)
        {
            Debug.LogError("You must specify an initial current state in the editor");
            return;
        }

        currentState.InitState(aiController);
    }

    public void Update()
    {
        currentState.UpdateState(aiController);
    }

    public void SetState(string stateName)
    {
        //Si el estado al que quiero cambiar, es en el que ya me encuentro
        //salgo del metodo.
        if (stateName == currentState.stateName)
            return;
        foreach (var state in states)
        {
            if (state.stateName == stateName)
            {
                if (currentState != null)
                {
                    currentState.EndState(aiController);
                }

                currentState = state;
                currentState.InitState(aiController);
                //aiController.Memory.Set("currentState", currentState.stateName);
                break;
            }
        }
    }
}
