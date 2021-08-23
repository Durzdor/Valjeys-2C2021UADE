using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private ThirdPersonController thirdPersonController;

    public Animator Animator => animator;
    public CharacterController Controller => characterController;
    public ThirdPersonController ThirdPersonController => thirdPersonController;

    private bool switchCharacter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (switchCharacter)
            {
                thirdPersonController.enabled = false;
                
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            var test = animator.GetCurrentAnimatorClipInfo(0);
            print(test[0].clip);
            print(test[0].clip.length);
            

        }
    }
}
