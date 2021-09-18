using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{

    #region SerializedFields
#pragma warning disable 649
    [Header("Lists")] [Space(5)]
    [SerializeField] private List<GameObject> pauseMenuScreens; // order: default, controls, options, help
    [SerializeField] private List<Button> uiButtons; // order: resume, controls, options, help, menu, quit, return, close
    [SerializeField] private List<Image> skillIcons;
    [SerializeField] private List<Image> skillCooldownFill;
    [SerializeField] private List<TextMeshProUGUI> skillHotkeys;

    [Header("GameObjects to activate")] [Space(5)] 
    [SerializeField] private GameObject messagePopup;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image damageTakenPlateVFX;
    [SerializeField] private Image checkpointUsed;

    [Header("Images")] [Space(5)] 
    [SerializeField] private Image healthBarFilling;
    [SerializeField] private Image manaBarFilling;
    [SerializeField] private Image experienceBarFilling;

    [Header("Texts")] [Space(5)] 
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI interactText;
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
    
    private void Update()
    {
        if (character.CharacterInput.GetPauseInput)
        {
            PauseMenuActivation();
        }
    }

    private void InteractTextHandler()
    {
        
    }
    
    private void FirstLoad()
    {
        characterMaxHp = character.CharacterHealth.MaxHealth;
        characterMaxMana = character.CharacterMana.MaxMana;
        characterExpToLevel = character.CharacterExperience.MaxExp;
        HealthBarUpdate(character.CharacterHealth.CurrentHealth, character.CharacterHealth.GetRatio);
        ManaBarUpdate(character.CharacterMana.CurrentMana,character.CharacterMana.GetRatio);
        ExperienceBarUpdate(character.CharacterExperience.CurrentExp, character.CharacterExperience.GetRatio);
        SkillHotkeysDisplay(character.CharacterInput.SkillHotkeys);
        NameChange(character.IsNaomi ? "Naomi" : "Ruth");
        LevelUpUpdate();
        SkillSwitch(character.IsNaomi ? character.CharacterNaomi.NaomiSkillImages : character.CharacterRuth.RuthSkillImages);
        CooldownFirstLoad();
    }

    private void CheckpointFeedback()
    {
        if (checkpointUsed.gameObject.activeInHierarchy) return;
        StartCoroutine(ImageFade(0.8f));
    }

    private IEnumerator ImageFade(float secondsToFade)
    {
        var timeElapsed = 0f;
        var lerpDuration = 0.5f;
        var currBlinks = 0;
        var blinkTimes = 3f;
        var nextColor = Color.white;
        checkpointUsed.gameObject.SetActive(true);
        while (timeElapsed<lerpDuration && currBlinks <= blinkTimes)
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
        checkpointUsed.color=Color.clear;
    }
    
    private void CooldownFirstLoad()
    {
        for (int i = 0; i < skillCooldownFill.Count; i++)
        {
            SkillCooldownUpdate(skillCooldownFill[i],0);
        }
    }
    private void CharacterEventSubscriptions()
    {
        character.CharacterHealth.OnDamaged += delegate { DamageTakenVFX(); HealthBarUpdate(character.CharacterHealth.CurrentHealth, character.CharacterHealth.GetRatio); };
        character.CharacterHealth.OnHealed += delegate { HealthBarUpdate(character.CharacterHealth.CurrentHealth, character.CharacterHealth.GetRatio); };
        character.CharacterMana.OnConsumed += delegate { ManaBarUpdate(character.CharacterMana.CurrentMana,character.CharacterMana.GetRatio); };
        character.CharacterMana.OnGained += delegate { ManaBarUpdate(character.CharacterMana.CurrentMana,character.CharacterMana.GetRatio); };
        character.CharacterRuth.OnRuthEnable += delegate { NameChange("Ruth"); SkillSwitch(character.CharacterRuth.RuthSkillImages); }; 
        character.CharacterNaomi.OnNaomiEnable += delegate { NameChange("Naomi"); SkillSwitch(character.CharacterNaomi.NaomiSkillImages); };
        character.CharacterExperience.OnExpGained += delegate { ExperienceBarUpdate(character.CharacterExperience.CurrentExp, character.CharacterExperience.GetRatio); };
        character.CharacterExperience.OnLevelUp += LevelUpUpdate;
        character.CharacterSkillController.OnSkill1Use += delegate(float f) { SkillCooldownUpdate(skillCooldownFill[0],f); };
        character.CharacterSkillController.OnSkill2Use += delegate(float f) { SkillCooldownUpdate(skillCooldownFill[1],f); };
        character.CharacterSkillController.OnSkill3Use += delegate(float f) { SkillCooldownUpdate(skillCooldownFill[2],f); };
        character.CharacterSkillController.OnSkill4Use += delegate(float f) { SkillCooldownUpdate(skillCooldownFill[3],f); };
        character.CharacterSkillController.OnSkill5Use += delegate(float f) { SkillCooldownUpdate(skillCooldownFill[4],f); };
        character.OnCharacterCheckpointUsed += CheckpointFeedback;
    }

    private void ButtonListeners()
    {
        // button order: resume, controls, options, help, quit menu, quit app, return
        uiButtons[0].onClick.AddListener(PauseMenuActivation);
        uiButtons[1].onClick.AddListener(delegate{SwitchPauseMenuScreen(1);});
        uiButtons[2].onClick.AddListener(delegate{SwitchPauseMenuScreen(2);});
        uiButtons[3].onClick.AddListener(delegate{SwitchPauseMenuScreen(3);});
        uiButtons[4].onClick.AddListener(QuitToMenu);
        uiButtons[5].onClick.AddListener(QuitToDesktop);
        uiButtons[6].onClick.AddListener(delegate{SwitchPauseMenuScreen(0);});
        uiButtons[7].onClick.AddListener(CloseMessagePopup);
    }

    private void SkillCooldownUpdate(Image cooldownFill, float cooldownRatio)
    {
        cooldownFill.fillAmount = cooldownRatio;
    }
    private void LevelUpUpdate()
    {
        characterExpToLevel = character.CharacterExperience.MaxExp;
        levelText.text = character.CharacterExperience.CurrentLevel.ToString();
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
        Time.timeScale = !currentState ? 0f : 1f;
    }

    private void CloseMessagePopup()
    {
        messagePopup.SetActive(false);
        Cursor.visible = false;
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
        SceneManager.LoadScene(menuScene);
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