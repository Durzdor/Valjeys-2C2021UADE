using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Switch")] [Space(5)]
    [SerializeField] private SkinnedMeshRenderer playerMesh;
    [SerializeField] private GameObject ruthGo;
    [SerializeField] private GameObject naomiGo;

    private Animator animator;
    private CharacterController characterController;
    private ThirdPersonController thirdPersonController;
    private CharacterAnimation characterAnimation;
    private CharacterInput characterInput;
    private CharacterSettings characterSettings;
    private Health characterHealth;
    private Mana characterMana;
    private Ruth characterRuth;
    private Naomi characterNaomi;
    private Experience characterExperience;
    private CharacterSkillController characterSkillController;

    public Animator Animator => animator;
    public CharacterController Controller => characterController;
    public ThirdPersonController ThirdPersonController => thirdPersonController;
    public CharacterInput CharacterInput => characterInput;
    public CharacterSettings CharacterSettings => characterSettings;
    public Health CharacterHealth => characterHealth;
    public Mana CharacterMana => characterMana;
    public Ruth CharacterRuth => characterRuth;
    public Naomi CharacterNaomi => characterNaomi;
    public Experience CharacterExperience => characterExperience;
    public CharacterSkillController CharacterSkillController => characterSkillController;

    public bool SwitchingCharacter { get; private set; }
    private bool isNaomi;

    public bool IsNaomi => isNaomi;

    public event Action OnCharacterSwitch;
    public event Action OnCharacterMoveLock;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        characterAnimation = GetComponent<CharacterAnimation>();
        characterInput = GetComponent<CharacterInput>();
        characterSettings = GetComponent<CharacterSettings>();
        characterHealth = GetComponent<Health>();
        characterMana = GetComponent<Mana>();
        characterRuth = ruthGo.GetComponent<Ruth>();
        characterNaomi = naomiGo.GetComponent<Naomi>();
        characterExperience = GetComponent<Experience>();
        characterSkillController = GetComponent<CharacterSkillController>();
    }

    private void Start()
    {
        characterAnimation.OnSwitchComplete += OnSwitchCompleteHandler;

    }

    private void Update()
    {
        if (SwitchingCharacter) return;
        // Character Switch InputDetection
        if (characterInput.GetSwitchCharacterInput)
        {
            if (!characterController.isGrounded) return;
            // Event for animations
            OnCharacterSwitch?.Invoke();
            SwitchStart();
        }
    }

    private void SwitchStart()
    {
        // turn on/off the required components for the switch
        SwitchingCharacter = true; 
        OnCharacterMoveLock?.Invoke();
        isNaomi = !isNaomi;

    }
    private void OnSwitchCompleteHandler()
    {
        SwitchingCharacter = false;
        if (isNaomi)
        {
            ruthGo.SetActive(false);
            naomiGo.SetActive(true);
            playerMesh.material = characterNaomi.NaomiMaterial;
        }
        else
        {
            ruthGo.SetActive(true);
            naomiGo.SetActive(false);
            playerMesh.material = characterRuth.RuthMaterial;
        }
        OnCharacterMoveLock?.Invoke();
    }
}
