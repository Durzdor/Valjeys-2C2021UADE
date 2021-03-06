using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [Header("Switch")] [Space(5)] 
    [SerializeField] private SkinnedMeshRenderer playerMesh;
    [SerializeField] private GameObject ruthGo;
    [SerializeField] private GameObject naomiGo;
    [SerializeField] private int orbsNeeded;
    [SerializeField] private Transform defaultCheckpoint;
    [Header("Sounds")] [Space(5)]
    [SerializeField] private AudioSource audioSwitchCharacter;
    [SerializeField] private AudioSource audioDeathCharacter;
#pragma warning restore 649

    #endregion

    #region ComponentReferences

    public CharacterAnimation Animation { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public CharacterMovement CharacterMovement { get; private set; }
    public CharacterInput Input { get; private set; }
    public CharacterSettings Settings { get; private set; }
    public Health Health { get; private set; }
    public Mana Mana { get; private set; }
    public Stamina Stamina { get; private set; }
    public Ruth Ruth { get; private set; }
    public Naomi Naomi { get; private set; }
    public Experience Experience { get; private set; }
    public CharacterSkillController SkillController { get; private set; }
    public CharacterUI Ui { get; private set; }
    public NotificationPopup NotificationPopup { get; private set; }
    public CharacterCamera Camera { get; private set; }

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
            OnCharacterInteractRange?.Invoke(Interactable != null ? Interactable.InteractionType : InteractionType.None);
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
    public event Action<InteractionType> OnCharacterInteractRange;
    public event Action<string,string> OnCharacterOrbAcquired;
    public event Action OnCharacterCheckpointUsed;
    public event Action OnCharacterPause;
    public event Action OnCharacterFadeIn;
    public event Action OnCharacterFadeOut;

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
        CharacterMovement = GetComponent<CharacterMovement>();
        Animation = GetComponent<CharacterAnimation>();
        Input = GetComponent<CharacterInput>();
        Settings = GetComponent<CharacterSettings>();
        Health = GetComponent<Health>();
        Mana = GetComponent<Mana>();
        Stamina = GetComponent<Stamina>();
        Ruth = ruthGo.GetComponent<Ruth>();
        Naomi = naomiGo.GetComponent<Naomi>();
        Experience = GetComponent<Experience>();
        SkillController = GetComponent<CharacterSkillController>();
        Ui = GetComponent<CharacterUI>();
        NotificationPopup = GetComponentInChildren<NotificationPopup>();
        Camera = GetComponent<CharacterCamera>();
        #endregion
    }

    private void Start()
    {
        Health.OnDeath += OnDeathHandler;
        Animation.OnSwitchComplete += OnSwitchCompleteHandler;
        Animation.OnDeathComplete += OnDeathCompleteHandler;
        Animation.OnInteractionComplete += CharacterInteraction;
        
        CheckpointRespawn = defaultCheckpoint;
        _orbsObtained = 0;
        IsNaomi = false;
        SwitchToRuth();
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
        }
        
        if (Input.GetPauseInput)
            OnCharacterPause?.Invoke();
        
        // Death Test
        if (UnityEngine.Input.GetKeyDown(KeyCode.L))
            Health.TakeDamage(9999);
        // Orb Test
        if (UnityEngine.Input.GetKeyDown(KeyCode.F6))
        {
            OrbsObtained = 3;
        }
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
        audioDeathCharacter.Play();
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
        Stamina.ResetToMax();
    }

    public void Teleport(Transform pos)
    {
        if (_isTeleporting) return;
        // TODO: poner algo para teleport overlay (Fade a negro?)
        OnCharacterFadeIn?.Invoke();
        StartCoroutine(TeleportTo(pos));
    }

    private IEnumerator TeleportTo(Transform pos)
    {
        _isTeleporting = true;
        CharacterMovement.enabled = false;
        var playerTransform = transform;
        playerTransform.position = pos.position;
        playerTransform.rotation = pos.rotation;
        yield return new WaitForSeconds(0.15f);
        CharacterMovement.enabled = true;
        _isTeleporting = false;
        // TODO: sacar el teleport overlay
       
    }

    private void SwitchStart()
    {
        // turn on/off the required components for the switch
        IsAnimationLocked = true;
        IsNaomi = !IsNaomi;
    }

    private void OnSwitchCompleteHandler()
    {
        IsAnimationLocked = false;
        audioSwitchCharacter.Play();
        if (IsNaomi)
            SwitchToNaomi();
        else
            SwitchToRuth();
    }

    private void SwitchToNaomi()
    {
        ruthGo.SetActive(false);
        naomiGo.SetActive(true);
        playerMesh.material = Naomi.NaomiMaterial;
        Animator.SetBool(NaomiBool,true);
    }

    private void SwitchToRuth()
    {
        ruthGo.SetActive(true);
        naomiGo.SetActive(false);
        playerMesh.material = Ruth.RuthMaterial;
        Animator.SetBool(NaomiBool,false);
    }
}