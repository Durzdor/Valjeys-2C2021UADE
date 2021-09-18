using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class Interactable : MonoBehaviour, IInteractable
{
    [CanBeNull] protected Character Character;
    protected string InteractableName;
    private string interactionHotkey;

    public event Action<string> OnInteractTextDisplay; 

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
            interactionHotkey = Character.CharacterInput.Interact;
            // string $"Press {interactionHotkey} to use {InteractableName}";
            OnInteractTextDisplay?.Invoke($"Press {interactionHotkey} to use {InteractableName}");
            Character.IsInInteractRange = true;
            Character.Interactable = this;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Character == null) return;
            Character.IsInInteractRange = false;
            Character.Interactable = null;
            Character = null;
        } 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.localPosition + GetComponent<SphereCollider>().center,GetComponent<SphereCollider>().radius);
    }
}
