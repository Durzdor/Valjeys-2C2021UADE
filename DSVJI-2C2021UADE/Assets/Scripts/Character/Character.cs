using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [Header("Switch")] [Space(5)] [SerializeField]
    private SkinnedMeshRenderer playerMesh;

    [SerializeField] private GameObject ruthGo;
    [SerializeField] private GameObject naomiGo;
#pragma warning restore 649

    #endregion

    #region References

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

    #endregion

    private bool isNaomi;
    private bool isTeleporting;
    private Transform checkpointRespawn;
    
    public bool IsAnimationLocked { get; private set; }
    public bool IsNaomi => isNaomi;
    public bool IsInInteractRange { get; set; }
    [CanBeNull] public IInteractable Interactable { get; set; }

    public event Action OnCharacterSwitch;
    public event Action OnCharacterInteract;
    public event Action OnCharacterCheckpointUsed;

    private void Awake()
    {
        #region Reference GetComponent

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

        #endregion
    }

    private void Start()
    {
        characterHealth.OnDeath += OnDeathHandler;
        characterAnimation.OnSwitchComplete += OnSwitchCompleteHandler;
        characterAnimation.OnDeathComplete += OnDeathCompleteHandler;
        // starting values
        checkpointRespawn = transform;
    }

    private void Update()
    {
        if (characterHealth.IsDead)
        {
            IsAnimationLocked = true;
        }

        // Character Switch InputDetection
        if (characterInput.GetSwitchCharacterInput)
        {
            // Event for animations
            OnCharacterSwitch?.Invoke();
            SwitchStart();
        }

        // Interaction input detection
        if (characterInput.GetInteractInput && IsInInteractRange)
        {
            OnCharacterInteract?.Invoke();
            CharacterInteraction();
        }

        // Death Test
        if (Input.GetKeyDown(KeyCode.L))
        {
            characterHealth.TakeDamage(9999);
        }
    }

    public void SaveCheckpoint(Transform t)
    {
        OnCharacterCheckpointUsed?.Invoke();
        checkpointRespawn = t;
        FillStats();
    }

    private void CharacterInteraction()
    {
        Interactable?.Interaction();
    }

    private void OnDeathHandler()
    {
        IsAnimationLocked = true;
    }

    private void OnDeathCompleteHandler()
    {
        IsAnimationLocked = false;
        FillStats();
        Teleport(checkpointRespawn);
    }

    private void FillStats()
    {
        characterMana.ResetToMax();
        characterHealth.ResetToMax();
    }

    public void Teleport(Transform pos)
    {
        if (isTeleporting) return;
        // TODO: poner algo para teleport overlay (Fade a negro?)
        StartCoroutine(TeleportTo(pos));
    }

    private IEnumerator TeleportTo(Transform pos)
    {
        isTeleporting = true;
        thirdPersonController.enabled = false;
        transform.position = pos.position;
        transform.rotation = pos.rotation;
        yield return new WaitForSeconds(0.15f);
        thirdPersonController.enabled = true;
        isTeleporting = false;
        // TODO: sacar el teleport overlay
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