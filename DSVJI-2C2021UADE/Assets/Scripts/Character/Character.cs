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
    [SerializeField] private int orbsNeeded;
#pragma warning restore 649

    #endregion

    #region References

    private Animator animator;
    private CharacterController characterController;
    private ThirdPersonController thirdPersonController;
    private CharacterAnimation characterAnimation;
    private CharacterInput input;
    private CharacterSettings settings;
    private Health health;
    private Mana mana;
    private Ruth ruth;
    private Naomi naomi;
    private Experience experience;
    private CharacterSkillController skillController;
    private CharacterUI ui;

    public Animator Animator => animator;
    public CharacterController Controller => characterController;
    public ThirdPersonController ThirdPersonController => thirdPersonController;
    public CharacterInput Input => input;
    public CharacterSettings Settings => settings;
    public Health Health => health;
    public Mana Mana => mana;
    public Ruth Ruth => ruth;
    public Naomi Naomi => naomi;
    public Experience Experience => experience;
    public CharacterSkillController SkillController => skillController;
    public CharacterUI Ui => ui;

    #endregion

    private bool isNaomi;
    private bool isTeleporting;
    private Transform checkpointRespawn;
    private bool isInInteractRange;
    private int orbsObtained;

    public bool IsAnimationLocked { get; set; }
    public bool IsNaomi => isNaomi;
    public bool IsInInteractRange
    {
        get => isInInteractRange;
        set
        {
            isInInteractRange = value;
            if (!(Interactable is null))
            {
                OnCharacterInteractRange?.Invoke($"Press {input.Interact} to use {Interactable.Name}");
            }
            else
            {
                OnCharacterInteractRange?.Invoke(null);
            }    
        } 
    }

    public int OrbsObtained
    {
        get => orbsObtained;
        private set
        {
            orbsObtained = value;
            if (orbsObtained >= orbsNeeded)
            {
                orbsObtained = orbsNeeded;
                if (!(Interactable is null))
                    OnCharacterOrbAcquired?.Invoke("All orbs acquired","Now go to the altar to finish the ritual");
            }
            else
            {
                if (!(Interactable is null))
                    OnCharacterOrbAcquired?.Invoke("Orb acquired", $"You got {Interactable.Name}\nNow you need {orbsNeeded-orbsObtained} more");
            }
        } 
    }

    [CanBeNull] public Interactable Interactable { get; set; }

    public Transform CheckpointRespawn => checkpointRespawn;

    public event Action OnCharacterSwitch;
    public event Action OnCharacterInteract;
    public event Action<string> OnCharacterInteractRange;
    public event Action<string,string> OnCharacterOrbAcquired;
    public event Action OnCharacterCheckpointUsed;
    public event Action OnCharacterPause;

    private void Awake()
    {
        #region Reference GetComponent

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        characterAnimation = GetComponent<CharacterAnimation>();
        input = GetComponent<CharacterInput>();
        settings = GetComponent<CharacterSettings>();
        health = GetComponent<Health>();
        mana = GetComponent<Mana>();
        ruth = ruthGo.GetComponent<Ruth>();
        naomi = naomiGo.GetComponent<Naomi>();
        experience = GetComponent<Experience>();
        skillController = GetComponent<CharacterSkillController>();
        ui = GetComponent<CharacterUI>();

        #endregion
    }

    private void Start()
    {
        health.OnDeath += OnDeathHandler;
        characterAnimation.OnSwitchComplete += OnSwitchCompleteHandler;
        characterAnimation.OnDeathComplete += OnDeathCompleteHandler;
        // starting values
        checkpointRespawn = transform;
        orbsObtained = 0;
    }

    private void Update()
    {
        if (health.IsDead)
        {
            IsAnimationLocked = true;
        }
        
        // Character Switch InputDetection
        if (input.GetSwitchCharacterInput)
        {
            // Event for animations
            OnCharacterSwitch?.Invoke();
            SwitchStart();
        }

        // Interaction input detection
        if (input.GetInteractInput && IsInInteractRange)
        {
            OnCharacterInteract?.Invoke();
            CharacterInteraction();
        }
        
        // Pause menu input detection
        if (Input.GetPauseInput)
        {
            OnCharacterPause?.Invoke();
        }

        // Death Test
        if (UnityEngine.Input.GetKeyDown(KeyCode.L))
        {
            health.TakeDamage(9999);
        }
    }

    public void OrbAcquisition()
    {
        OrbsObtained++;
        if (!(Interactable is null)) Interactable.gameObject.SetActive(false);
    }

    public void SaveCheckpoint(Transform t)
    {
        OnCharacterCheckpointUsed?.Invoke();
        checkpointRespawn = t;
        FillStats();
    }

    private void CharacterInteraction()
    {
        if (!(Interactable is null)) Interactable.Interaction();
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
        mana.ResetToMax();
        health.ResetToMax();
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
            playerMesh.material = naomi.NaomiMaterial;
        }
        else
        {
            ruthGo.SetActive(true);
            naomiGo.SetActive(false);
            playerMesh.material = ruth.RuthMaterial;
        }
    }
}