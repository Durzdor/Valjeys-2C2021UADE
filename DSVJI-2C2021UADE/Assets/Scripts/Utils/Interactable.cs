using JetBrains.Annotations;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class Interactable : MonoBehaviour, IInteractable
{
    [CanBeNull] protected Character Character;
    protected string InteractableName;
    private TextMeshProUGUI interactionText;
    private string interactionHotkey;

    #region SerializedFields
#pragma warning disable 649
    [SerializeField] private GameObject canvas;
#pragma warning restore 649
    #endregion

    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        interactionText = canvas.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (canvas.activeInHierarchy)
        {
            if (!(Camera.main is null))
            {
                canvas.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
            }
        }
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
            interactionText.text = $"Press {interactionHotkey} to use {InteractableName}";
            Character.IsInInteractRange = true;
            Character.Interactable = this;
            canvas.SetActive(true);
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
            canvas.SetActive(false);
        } 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,GetComponent<SphereCollider>().radius);
    }
}
