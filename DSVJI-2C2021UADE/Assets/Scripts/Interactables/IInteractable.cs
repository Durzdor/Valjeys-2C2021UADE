using UnityEngine;

public interface IInteractable
{
    void Interaction();
    void OnTriggerEnter(Collider other);
    void OnTriggerExit(Collider other);
}
