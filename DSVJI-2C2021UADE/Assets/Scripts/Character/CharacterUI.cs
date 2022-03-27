using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [Header("GameObjects to activate")] [Space(5)]
    [SerializeField] private Image damageTakenPlateVFX;
    [SerializeField] private Image damageTakenOverlayVFX;
    [SerializeField] private Image checkpointUsed;
    [SerializeField] private GameObject ruthMainIcon;
    [SerializeField] private GameObject naomiMainIcon;

    [Header("Fill Bars")] [Space(5)] 
    [SerializeField] private Image healthBarFilling;
    [SerializeField] private Image manaBarFilling;
    [SerializeField] private Image staminaBarFilling;
    [SerializeField] private Image experienceBarFilling;

    [Header("Texts")] [Space(5)] 
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI switchKeyText;
    
    [Header("Assignable")][Space(5)] 
    [SerializeField] private GameObject fadeGameObject;
    [SerializeField] private Animator fadeAnimator;
#pragma warning restore 649

    #endregion
    
    private Character _character;
    private float _characterMaxHp;
    private float _characterMaxMana;
    private float _characterMaxStamina;
    private float _characterExpToLevel;
    private static readonly int FadeIn = Animator.StringToHash("FadeIn");
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private void Start()
    {
        FirstLoad();
        CharacterEventSubscriptions();
    }

    private void FirstLoad()
    {
        _characterMaxHp = _character.Health.MaxHealth;
        _characterMaxMana = _character.Mana.MaxMana;
        _characterMaxStamina = _character.Stamina.MaxStamina;
        _characterExpToLevel = _character.Experience.MaxExp;
        HealthBarUpdate(_character.Health.CurrentHealth, _character.Health.GetRatio);
        ManaBarUpdate(_character.Mana.CurrentMana, _character.Mana.GetRatio);
        StaminaBarUpdate(_character.Stamina.CurrentStamina, _character.Stamina.GetRatio);
        ExperienceBarUpdate(_character.Experience.CurrentExp, _character.Experience.GetRatio);
        NameChange(_character.IsNaomi ? "Naomi" : "Ruth");
        LevelUpUpdate();
        UpdateSwitchKey();
    }

    private void CheckpointFeedback()
    {
        if (checkpointUsed.gameObject.activeInHierarchy) return;
        StartCoroutine(ImageBlink());
    }

    private IEnumerator ImageBlink()
    {
        var timeElapsed = 0f;
        var lerpDuration = 0.5f;
        var currBlinks = 0;
        var blinkTimes = 3f;
        var nextColor = Color.white;
        checkpointUsed.gameObject.SetActive(true);
        
        while (timeElapsed < lerpDuration && currBlinks <= blinkTimes)
        {
            var lerp = Color.Lerp(checkpointUsed.color, nextColor, timeElapsed / lerpDuration);
            checkpointUsed.color = lerp;
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= lerpDuration)
            {
                currBlinks++;
                timeElapsed = 0f;
                nextColor = checkpointUsed.color == Color.white ? Color.clear : Color.white;
            }

            yield return null;
        }

        checkpointUsed.gameObject.SetActive(false);
        checkpointUsed.color = Color.clear;
    }
    
    private void CharacterEventSubscriptions()
    {
        _character.Health.OnConsumed += delegate
        {
            DamageTakenVFX();
            HealthBarUpdate(_character.Health.CurrentHealth, _character.Health.GetRatio);
        };
        _character.Health.OnGained += delegate
        {
            HealthBarUpdate(_character.Health.CurrentHealth, _character.Health.GetRatio);
        };
        _character.Mana.OnConsumed += delegate 
            { ManaBarUpdate(_character.Mana.CurrentMana, _character.Mana.GetRatio); };
        _character.Mana.OnGained += delegate 
            { ManaBarUpdate(_character.Mana.CurrentMana, _character.Mana.GetRatio); };
        _character.Stamina.OnGained += delegate 
            {StaminaBarUpdate(_character.Stamina.CurrentStamina, _character.Stamina.GetRatio); };
        _character.Stamina.OnConsumed += delegate 
            { StaminaBarUpdate(_character.Stamina.CurrentStamina, _character.Stamina.GetRatio); };
        _character.Ruth.OnRuthEnable += delegate
        {
            NameChange("Ruth");
            RuthMainIcon();
        };
        _character.Naomi.OnNaomiEnable += delegate
        {
            NameChange("Naomi");
            NaomiMainIcon();
        };
        _character.Experience.OnExpGained += delegate
        {
            ExperienceBarUpdate(_character.Experience.CurrentExp, _character.Experience.GetRatio);
        };
        _character.Experience.OnLevelUp += LevelUpUpdate;
        _character.OnCharacterCheckpointUsed += CheckpointFeedback;
        _character.OnCharacterFadeIn += FadeInHandler;
        _character.OnCharacterFadeOut += FadeOutHandler;
    }

    private void LevelUpUpdate()
    {
        _characterExpToLevel = _character.Experience.MaxExp;
        levelText.text = _character.Experience.CurrentLevel.ToString();
        if (Mathf.Approximately(_characterExpToLevel, float.MaxValue))
        {
            experienceText.gameObject.SetActive(false);
        }
    }

    private void NameChange(string newName)
    {
        nameText.text = newName;
    }

    private void DamageTakenVFX()
    {
        StartCoroutine(DamageVFXWait());
    }

    private IEnumerator DamageVFXWait()
    {
        damageTakenPlateVFX.gameObject.SetActive(true);
        damageTakenOverlayVFX.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        damageTakenPlateVFX.gameObject.SetActive(false);
        damageTakenOverlayVFX.gameObject.SetActive(false);
    }
    
    private void ExperienceBarUpdate(float currExp, float expPercent)
    {
        experienceBarFilling.fillAmount = expPercent;
        experienceText.text = $"{currExp} / {_characterExpToLevel}";
    }

    private void StaminaBarUpdate(float currStamina, float staminaPercent)
    {
        staminaBarFilling.fillAmount = staminaPercent;
        staminaText.text = $"{currStamina} / {_characterMaxStamina}";
    }
    
    private void ManaBarUpdate(float currMana, float manaPercent)
    {
        manaBarFilling.fillAmount = manaPercent;
        manaText.text = $"{currMana} / {_characterMaxMana}";
    }

    private void HealthBarUpdate(float currHp, float hpPercent)
    {
        healthBarFilling.fillAmount = hpPercent;
        healthText.text = $"{currHp} / {_characterMaxHp}";
    }

    private void RuthMainIcon()
    {
        ruthMainIcon.SetActive(true);
        naomiMainIcon.SetActive(false);
    }
    private void NaomiMainIcon()
    {
        naomiMainIcon.SetActive(true);
        ruthMainIcon.SetActive(false);
    }

    private void UpdateSwitchKey()
    {
        switchKeyText.text = _character.Input.KeyBindData.switchCharacter.ToString();
    }

    public void FadeInHandler()
    {
        fadeGameObject.SetActive(true);
        fadeAnimator.SetTrigger(FadeIn);
    }
    public void FadeOutHandler()
    {
        fadeGameObject.SetActive(true);
        fadeAnimator.SetTrigger(FadeOut);
    }
}