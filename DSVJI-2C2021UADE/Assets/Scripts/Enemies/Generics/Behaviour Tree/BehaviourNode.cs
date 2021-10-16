using UnityEngine;
using System.Collections;


// Enums de qué devuelve el nodo.
public enum BehaviourResult {None, Success, Failure, Processing}

public abstract class BehaviourNode : MonoBehaviour
{
    private BehaviourResult lastResult = BehaviourResult.None;

    public BehaviourResult LastResult => lastResult;

    // Funcion para iniciar el nodo
    public virtual void Init(AIController ai){}
    
    // Funcion para terminar el nodo
    public virtual void End(AIController ai){}

    public virtual bool CanExecute(AIController ai) => true;

    protected abstract BehaviourResult ExecuteInternal(AIController ai);
    
    public BehaviourResult Execute(AIController ai)
    {
        if (lastResult != BehaviourResult.Processing)
            Init(ai);
        var newResult = ExecuteInternal(ai);
        lastResult = newResult;

        if(newResult != BehaviourResult.Processing)
            End(ai);

        return newResult;
    }
}