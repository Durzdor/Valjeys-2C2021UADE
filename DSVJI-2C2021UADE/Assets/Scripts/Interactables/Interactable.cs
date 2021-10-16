using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class Interactable : MonoBehaviour, IInteractable
{
    [CanBeNull] protected Character Character;
    protected string InteractableName;
    public string Name => InteractableName;
    
    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    public virtual void Interaction()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Character = other.GetComponent<Character>();
            if (Character == null) return;
            Character.Interactable = this;
            Character.IsInInteractRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterClear();
        } 
    }

    private void CharacterClear()
    {
        if (Character == null) return;
        Character.Interactable = null;
        Character.IsInInteractRange = false;
        Character = null;
    }
    
    private void OnDisable()
    {
        CharacterClear();
    }
}
