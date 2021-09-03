using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer playerMesh;
    [SerializeField] private Material ruthMaterial;
    [SerializeField] private Material naomiMaterial;    
    
    private Animator animator;
    private CharacterController characterController;
    private ThirdPersonController thirdPersonController;
    private CharacterAnimation characterAnimation;

    public Animator Animator => animator;
    public CharacterController Controller => characterController;
    public ThirdPersonController ThirdPersonController => thirdPersonController;

    private bool switchingCharacter;
    private bool isNaomi;

    public event Action OnCharacterSwitch;
    public event Action OnCharacterMoveLock;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        characterAnimation = GetComponent<CharacterAnimation>();

        characterAnimation.OnSwitchComplete += OnSwitchCompleteHandler;
    }

    private void Update()
    {
        // Character Switch InputDetection
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!characterController.isGrounded || switchingCharacter) return;
            // Event for animations
            OnCharacterSwitch?.Invoke();
            SwitchStart();
        }
    }

    private void SwitchStart()
    {
        // turn on/off the required components for the switch
        switchingCharacter = true; 
        OnCharacterMoveLock?.Invoke();
        isNaomi = !isNaomi;

    }
    private void OnSwitchCompleteHandler()
    {
        switchingCharacter = false;
        if (isNaomi)
        {
            playerMesh.material = naomiMaterial;
        }
        else
        {
            playerMesh.material = ruthMaterial;
        }
        OnCharacterMoveLock?.Invoke();
    }
}
