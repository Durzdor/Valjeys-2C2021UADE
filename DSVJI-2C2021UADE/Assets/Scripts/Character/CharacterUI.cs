using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [Header("Lists")] [Space(5)] [SerializeField]
    private List<GameObject> pauseMenuScreens; // order: default, controls, options, help

    [SerializeField]
    private List<Button> uiButtons; // order: resume, controls, options, help, menu, quit, return, close

    [SerializeField] private List<Image> skillIcons;
    [SerializeField] private List<Image> skillCooldownFill;
    [SerializeField] private List<TextMeshProUGUI> skillHotkeys;

    [Header("GameObjects to activate")] [Space(5)] [SerializeField]
    private GameObject messagePopup;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image damageTakenPlateVFX;
    [SerializeField] private Image checkpointUsed;

    [Header("Images")] [Space(5)] [SerializeField]
    private Image healthBarFilling;

    [SerializeField] private Image manaBarFilling;
    [SerializeField] private Image experienceBarFilling;

    [Header("Texts")] [Space(5)] [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI messagePopupTitleText;
    [SerializeField] private TextMeshProUGUI messagePopupBodyText;
#pragma warning restore 649

    #endregion

    private const string menuScene = "MainMenu"; // menu scene name

    private Character character;
    private float characterMaxHp;
    private float characterMaxMana;
    private float characterExpToLevel;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        FirstLoad();
        CharacterEventSubscriptions();
        ButtonListeners();
    }

    private void MessagePopupHandler(string title, string body)
    {
        messagePopup.SetActive(true);
        character.IsAnimationLocked = true;
        messagePopupTitleText.text = title;
        messagePopupBodyText.text = body;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
    }

    private void InteractTextHandler([CanBeNull] string text)
    {
        if (text != null)
        {
            interactText.gameObject.SetActive(true);
            interactText.text = text;
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }

    private void FirstLoad()
    {
        characterMaxHp = character.Health.MaxHealth;
        characterMaxMana = character.Mana.MaxMana;
        characterExpToLevel = character.Experience.MaxExp;
        HealthBarUpdate(character.Health.CurrentHealth, character.Health.GetRatio);
        ManaBarUpdate(character.Mana.CurrentMana, character.Mana.GetRatio);
        ExperienceBarUpdate(character.Experience.CurrentExp, character.Experience.GetRatio);
        SkillHotkeysDisplay(character.Input.SkillHotkeys);
        NameChange(character.IsNaomi ? "Naomi" : "Ruth");
        LevelUpUpdate();
        SkillSwitch(character.IsNaomi ? character.Naomi.NaomiSkillImages : character.Ruth.RuthSkillImages);
        CooldownFirstLoad();
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

    private void CooldownFirstLoad()
    {
        for (int i = 0; i < skillCooldownFill.Count; i++)
        {
            SkillCooldownUpdate(skillCooldownFill[i], 0);
        }
    }

    private void CharacterEventSubscriptions()
    {
        character.Health.OnDamaged += delegate
        {
            DamageTakenVFX();
            HealthBarUpdate(character.Health.CurrentHealth, character.Health.GetRatio);
        };
        character.Health.OnHealed += delegate
        {
            HealthBarUpdate(character.Health.CurrentHealth, character.Health.GetRatio);
        };
        character.Mana.OnConsumed += delegate { ManaBarUpdate(character.Mana.CurrentMana, character.Mana.GetRatio); };
        character.Mana.OnGained += delegate { ManaBarUpdate(character.Mana.CurrentMana, character.Mana.GetRatio); };
        character.Ruth.OnRuthEnable += delegate
        {
            NameChange("Ruth");
            SkillSwitch(character.Ruth.RuthSkillImages);
        };
        character.Naomi.OnNaomiEnable += delegate
        {
            NameChange("Naomi");
            SkillSwitch(character.Naomi.NaomiSkillImages);
        };
        character.Experience.OnExpGained += delegate
        {
            ExperienceBarUpdate(character.Experience.CurrentExp, character.Experience.GetRatio);
        };
        character.Experience.OnLevelUp += LevelUpUpdate;
        character.SkillController.OnSkill1Use += delegate(float f) { SkillCooldownUpdate(skillCooldownFill[0], f); };
        character.SkillController.OnSkill2Use += delegate(float f) { SkillCooldownUpdate(skillCooldownFill[1], f); };
        character.SkillController.OnSkill3Use += delegate(float f) { SkillCooldownUpdate(skillCooldownFill[2], f); };
        character.SkillController.OnSkill4Use += delegate(float f) { SkillCooldownUpdate(skillCooldownFill[3], f); };
        character.SkillController.OnSkill5Use += delegate(float f) { SkillCooldownUpdate(skillCooldownFill[4], f); };
        character.OnCharacterCheckpointUsed += CheckpointFeedback;
        character.OnCharacterInteractRange += delegate(string s) { InteractTextHandler(s); };
        character.OnCharacterPause += PauseMenuActivation;
        character.OnCharacterOrbAcquired += delegate(string title, string body) { MessagePopupHandler(title, body); };
    }

    private void ButtonListeners()
    {
        // button order: resume, controls, options, help, quit menu, quit app, return
        uiButtons[0].onClick.AddListener(PauseMenuActivation);
        uiButtons[1].onClick.AddListener(delegate { SwitchPauseMenuScreen(1); });
        uiButtons[2].onClick.AddListener(delegate { SwitchPauseMenuScreen(2); });
        uiButtons[3].onClick.AddListener(delegate { SwitchPauseMenuScreen(3); });
        uiButtons[4].onClick.AddListener(QuitToMenu);
        uiButtons[5].onClick.AddListener(QuitToDesktop);
        uiButtons[6].onClick.AddListener(delegate { SwitchPauseMenuScreen(0); });
        uiButtons[7].onClick.AddListener(CloseMessagePopup);
    }

    private void SkillCooldownUpdate(Image cooldownFill, float cooldownRatio)
    {
        cooldownFill.fillAmount = cooldownRatio;
    }

    private void LevelUpUpdate()
    {
        characterExpToLevel = character.Experience.MaxExp;
        levelText.text = character.Experience.CurrentLevel.ToString();
        if (Mathf.Approximately(characterExpToLevel, float.MaxValue))
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
        yield return new WaitForSeconds(0.1f);
        damageTakenPlateVFX.gameObject.SetActive(false);
    }

    private void SkillHotkeysDisplay(List<string> newHotkeys)
    {
        for (int i = 0; i < skillHotkeys.Count; i++)
        {
            skillHotkeys[i].text = newHotkeys[i];
        }
    }

    private void SkillSwitch(List<Sprite> newIcons)
    {
        for (int i = 0; i < newIcons.Count; i++)
        {
            skillIcons[i].gameObject.SetActive(true);
            skillIcons[i].sprite = newIcons[i];
        }

        for (int i = newIcons.Count; i < skillIcons.Count; i++)
        {
            skillIcons[i].gameObject.SetActive(false);
        }
    }

    private void ExperienceBarUpdate(float currExp, float expPercent)
    {
        experienceBarFilling.fillAmount = expPercent;
        experienceText.text = $"{currExp} / {characterExpToLevel}";
    }

    private void ManaBarUpdate(float currMana, float manaPercent)
    {
        manaBarFilling.fillAmount = manaPercent;
        manaText.text = $"{currMana} / {characterMaxMana}";
    }

    private void HealthBarUpdate(float currHp, float hpPercent)
    {
        healthBarFilling.fillAmount = hpPercent;
        healthText.text = $"{currHp} / {characterMaxHp}";
    }

    private void PauseMenuActivation()
    {
        var currentState = pauseMenu.activeInHierarchy;
        pauseMenu.SetActive(!currentState);
        Cursor.visible = !currentState;
        Cursor.lockState = !currentState ? CursorLockMode.Confined : CursorLockMode.Locked;
        Time.timeScale = !currentState ? 0f : 1f;
        character.IsAnimationLocked = !currentState;
    }

    private void CloseMessagePopup()
    {
        messagePopup.SetActive(false);
        character.IsAnimationLocked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    private void SwitchPauseMenuScreen(int screenToShow)
    {
        for (int i = 0; i < pauseMenuScreens.Count; i++)
        {
            pauseMenuScreens[i].gameObject.SetActive(i == screenToShow);
        }
    }

    private void QuitToMenu()
    {
        SceneManager.LoadSceneAsync(menuScene);
        SwitchPauseMenuScreen(0);
        PauseMenuActivation();
        Time.timeScale = 1f;
    }

    private void QuitToDesktop()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE_WIN
        Application.Quit();
#endif
    }
}