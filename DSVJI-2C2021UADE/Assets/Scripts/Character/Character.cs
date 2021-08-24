using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private ThirdPersonController thirdPersonController;
    private CharacterAnimation characterAnimation;

    public Animator Animator => animator;
    public CharacterController Controller => characterController;
    public ThirdPersonController ThirdPersonController => thirdPersonController;

    private bool switchCharacter;

    public event Action OnCharacterSwitch;

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
            if (!characterController.isGrounded || switchCharacter) return;
            // Event for animations
            OnCharacterSwitch?.Invoke();
            SwitchStart();
        }
    }

    private void SwitchStart()
    {
        // turn on/off the required components for the switch
        switchCharacter = true; 
        thirdPersonController.enabled = false; 
    }
    private void OnSwitchCompleteHandler()
    {
        switchCharacter = false;
        thirdPersonController.enabled = true;
    }
}
