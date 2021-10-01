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
    [SerializeField] private Transform defaultCheckpoint;
    
#pragma warning restore 649

    #endregion

    #region ComponentReferences

    public CharacterAnimation Animation { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public ThirdPersonController ThirdPersonController { get; private set; }
    public CharacterInput Input { get; private set; }
    public CharacterSettings Settings { get; private set; }
    public Health Health { get; private set; }
    public Mana Mana { get; private set; }
    public Ruth Ruth { get; private set; }
    public Naomi Naomi { get; private set; }
    public Experience Experience { get; private set; }
    public CharacterSkillController SkillController { get; private set; }
    public CharacterUI Ui { get; private set; }
    public NotificationPopup NotificationPopup { get; private set; }

    #endregion

    #region PublicProperty

    public bool IsAnimationLocked { get; set; }
    public bool IsNaomi { get; private set; }
    public bool IsInInteractRange
    {
        get => _isInInteractRange;
        set
        {
            _isInInteractRange = value;
            OnCharacterInteractRange?.Invoke(!(Interactable is null)
                ? $"Press {Input.Interact} to use {Interactable.Name}"
                : null);
        } 
    }
    public int OrbsObtained
    {
        get => _orbsObtained;
        private set
        {
            _orbsObtained = value;
            if (_orbsObtained >= orbsNeeded)
            {
                _orbsObtained = orbsNeeded;
                GotAllOrbs = true;
                if (!(Interactable is null))
                    OnCharacterOrbAcquired?.Invoke("All orbs acquired","Now go to the altar to finish the ritual");
            }
            else
            {
                GotAllOrbs = false;
                if (!(Interactable is null))
                    OnCharacterOrbAcquired?.Invoke("Orb acquired", $"You got {Interactable.Name}\nNow you need {orbsNeeded-_orbsObtained} more");
            }
        } 
    }
    public bool GotAllOrbs { get; private set; }
    public int OrbsNeeded => orbsNeeded;
    public Transform CheckpointRespawn { get; private set; }
    [CanBeNull] public Interactable Interactable { get; set; }

    #endregion

    #region Events

    public event Action OnCharacterSwitch;
    public event Action OnCharacterInteract;
    public event Action<string> OnCharacterInteractRange;
    public event Action<string,string> OnCharacterOrbAcquired;
    public event Action OnCharacterCheckpointUsed;
    public event Action OnCharacterPause;

    #endregion
    
    private bool _isTeleporting;
    private bool _isInInteractRange;
    private int _orbsObtained;

    private static readonly int NaomiBool = Animator.StringToHash("IsNaomi");

    private void Awake()
    {
        #region Reference GetComponent

        Animator = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();
        ThirdPersonController = GetComponent<ThirdPersonController>();
        Animation = GetComponent<CharacterAnimation>();
        Input = GetComponent<CharacterInput>();
        Settings = GetComponent<CharacterSettings>();
        Health = GetComponent<Health>();
        Mana = GetComponent<Mana>();
        Ruth = ruthGo.GetComponent<Ruth>();
        Naomi = naomiGo.GetComponent<Naomi>();
        Experience = GetComponent<Experience>();
        SkillController = GetComponent<CharacterSkillController>();
        Ui = GetComponent<CharacterUI>();
        NotificationPopup = GetComponentInChildren<NotificationPopup>();

        #endregion
    }

    private void Start()
    {
        Health.OnDeath += OnDeathHandler;
        Animation.OnSwitchComplete += OnSwitchCompleteHandler;
        Animation.OnDeathComplete += OnDeathCompleteHandler;
        
        CheckpointRespawn = transform;
        _orbsObtained = 0;
    }

    private void Update()
    {
        if (Health.IsDead)
            IsAnimationLocked = true;
        
        if (Input.GetSwitchCharacterInput)
        {
            OnCharacterSwitch?.Invoke();
            SwitchStart();
        }
        
        if (Input.GetInteractInput && IsInInteractRange)
        {
            OnCharacterInteract?.Invoke();
            CharacterInteraction();
        }
        
        if (Input.GetPauseInput)
            OnCharacterPause?.Invoke();
        
        // Death Test
        if (UnityEngine.Input.GetKeyDown(KeyCode.L))
            Health.TakeDamage(9999);
    }

    public void OrbAcquisition()
    {
        OrbsObtained++;
        if (!(Interactable is null)) Interactable.gameObject.SetActive(false);
    }

    public void SaveCheckpoint(Transform checkpointTransform)
    {
        OnCharacterCheckpointUsed?.Invoke();
        CheckpointRespawn = checkpointTransform;
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
        Teleport(CheckpointRespawn);
    }

    private void FillStats()
    {
        Mana.ResetToMax();
        Health.ResetToMax();
    }

    public void Teleport(Transform pos)
    {
        if (_isTeleporting) return;
        // TODO: poner algo para teleport overlay (Fade a negro?)
        StartCoroutine(TeleportTo(pos));
    }

    private IEnumerator TeleportTo(Transform pos)
    {
        _isTeleporting = true;
        ThirdPersonController.enabled = false;
        var playerTransform = transform;
        playerTransform.position = pos.position;
        playerTransform.rotation = pos.rotation;
        yield return new WaitForSeconds(0.15f);
        ThirdPersonController.enabled = true;
        _isTeleporting = false;
        // TODO: sacar el teleport overlay
    }

    private void SwitchStart()
    {
        // turn on/off the required components for the switch
        IsAnimationLocked = true;
        IsNaomi = !IsNaomi;
        Animator.SetBool(NaomiBool,IsNaomi);
    }

    private void OnSwitchCompleteHandler()
    {
        IsAnimationLocked = false;
        if (IsNaomi)
        {
            ruthGo.SetActive(false);
            naomiGo.SetActive(true);
            playerMesh.material = Naomi.NaomiMaterial;
        }
        else
        {
            ruthGo.SetActive(true);
            naomiGo.SetActive(false);
            playerMesh.material = Ruth.RuthMaterial;
        }
    }
}