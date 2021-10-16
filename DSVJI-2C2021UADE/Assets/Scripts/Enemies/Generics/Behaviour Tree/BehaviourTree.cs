using UnityEngine;

public class BehaviourTree : BehaviourNode
{
    BehaviourNode rootNode;
    AIController ai;
    
    void Awake()
    {
        //Obtengo el AI Controller.
        ai = GetComponentInParent<AIController>();

        if (ai == null)
        {
            Debug.LogError("Behaviour Tree should be placed as a child of an AIController");
        }
        
        if (transform.childCount == 1)
        {
            rootNode = transform.GetChild(0).GetComponent<BehaviourNode>();

            if (rootNode == null)
            {
                Debug.LogError("Root child doesnt have a BehaviorNode", gameObject);
            }
        }
        else
        {
            Debug.LogError("Root node should have only one child", gameObject);
        }
    }

    void Update()
    {
        Execute(ai);
    }

    protected override BehaviourResult ExecuteInternal(AIController ai)
    {
        if (rootNode != null)  
            return rootNode.Execute(ai);
        
        return BehaviourResult.Failure;
    }
}