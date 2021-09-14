using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Switch")] [Space(5)]
    [SerializeField] private SkinnedMeshRenderer playerMesh;
    [SerializeField] private GameObject ruthGo;
    [SerializeField] private GameObject naomiGo;
#pragma warning restore 649
    #endregion

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

    private bool isNaomi;
    private bool isTeleporting = false;
    
    [SerializeField] private Transform checkpointRespawn;
    public bool IsAnimationLocked { get; private set; }
    public bool IsNaomi => isNaomi;

    public event Action OnCharacterSwitch;

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
        characterHealth.OnDeath += OnDeathHandler;
        characterAnimation.OnSwitchComplete += OnSwitchCompleteHandler;
        characterAnimation.OnDeathComplete += OnDeathCompleteHandler;
    }

    private void Update()
    {
        if (characterHealth.IsDead)
        {
            IsAnimationLocked = true;
        }
        if (IsAnimationLocked) return;
        // Character Switch InputDetection
        if (characterInput.GetSwitchCharacterInput)
        {
            // Event for animations
            OnCharacterSwitch?.Invoke();
            SwitchStart();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            characterHealth.TakeDamage(9999);
        }
    }

    private void OnDeathHandler()
    {
        IsAnimationLocked = true;
    }
    private void OnDeathCompleteHandler()
    {
        IsAnimationLocked = false;
        characterMana.ResetToMax();
        characterHealth.ResetToMax();
        Teleport(checkpointRespawn);
    }

    public void Teleport(Transform pos)
    {
        if (isTeleporting) return;
        StartCoroutine(TeleportTo(pos));
    }

    private IEnumerator TeleportTo(Transform pos)
    {
        isTeleporting = true;
        thirdPersonController.enabled = false;
        transform.position = pos.position;
        transform.rotation = pos.rotation;
        yield return new WaitForSeconds(0.5f);
        thirdPersonController.enabled = true;
        isTeleporting = false;
    }

    private void SwitchStart()
    {
        // turn on/off the required components for the switch
        IsAnimationLocked = true; 
        isNaomi = !isNaomi;

    }
    private void OnSwitchCompleteHandler()
    {
        IsAnimationLocked = false;
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
    }
}
