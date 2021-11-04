using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class InteractionText : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Image interactImage;
    [SerializeField] private Sprite ruthIcon;
    [SerializeField] private Sprite naomiIcon;
#pragma warning restore 649

    #endregion

    private Character _character;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
    }

    private void Start()
    {
        _character.OnCharacterInteractRange += InteractTextHandler;
    }

    private void InteractTextHandler(InteractionType interactionType)
    {
        Debug.Assert(_character.Interactable != null, "_character.Interactable != null");
        switch (interactionType)
        {
            case InteractionType.None:
                interactText.gameObject.SetActive(false);
                interactImage.gameObject.SetActive(false);
                break;
            case InteractionType.Checkpoint:
                interactText.gameObject.SetActive(true);

                interactText.text =
                    $"Press {_character.Input.KeyBindData.interact} to rest at the {_character.Interactable.Name}";
                break;
            case InteractionType.Door:
                interactText.gameObject.SetActive(true);
                interactText.text =
                    $"Press {_character.Input.KeyBindData.interact} to open the {_character.Interactable.Name}";
                break;
            case InteractionType.Orb:
                interactText.gameObject.SetActive(true);
                interactText.text =
                    $"Press {_character.Input.KeyBindData.interact} to pickup {_character.Interactable.Name}";
                break;
            case InteractionType.SkillUnLocker:
                
                interactText.gameObject.SetActive(true);
                if (_character.Interactable.Name == "Chest")
                {
                    interactText.text =
                        $"Press {_character.Input.KeyBindData.interact} to open the Chest";
                }
                else
                {
                    interactImage.sprite = _character.Interactable.GetComponent<SkillUnLocker>().UnlockNaomiSkill ? naomiIcon : ruthIcon;
                    interactImage.gameObject.SetActive(true);
                    interactText.text =
                        $"Press {_character.Input.KeyBindData.interact} to unlock the {_character.Interactable.Name} skill";
                }
                break;
            case InteractionType.Teleport:
                interactText.gameObject.SetActive(true);
                interactText.text =
                    $"Press {_character.Input.KeyBindData.interact} to teleport: {_character.Interactable.Name}";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(interactionType), interactionType, null);
        }
    }
}