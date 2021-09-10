using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    /*
     * TODO:
     * scripts de ruth y naomi tiene que guardar las imagenes de las skills en una lista de sprites
     * hacer un inputmanager y guardar los hotkeys de las skills en una lista de strings
     * enganchar bien el character script asi puede agarrar la info que necesite
     * hacer eventos para diferentes cosas (vida cambia, mana cambia, exp cambia, recibe daño)
     */
    
    [Header("Lists")] [Space(5)]
    [SerializeField] private List<GameObject> pauseMenuScreens; // order: default, controls, options, help
    [SerializeField] private List<Button> uiButtons; // order: resume, controls, options, help, menu, quit, return, close
    [SerializeField] private List<Image> skillIcons;
    [SerializeField] private List<TextMeshProUGUI> skillHotkeys;

    [Header("GameObjects to activate")] [Space(5)] 
    [SerializeField] private GameObject messagePopup;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image damageTakenPlateVFX;

    [Header("Images")] [Space(5)] 
    [SerializeField] private Image healthBarFilling;
    [SerializeField] private Image manaBarFilling;
    [SerializeField] private Image experienceBarFilling;

    [Header("Texts")] [Space(5)] 
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI experienceText;

    private const string menuScene = "UiTest"; // menu scene name

    private int characterMaxHp;
    private int characterMaxMana;
    private int characterExpToLevel;

    private void Start()
    {
        // get max hp and mana from character
        // characterMaxHp = 10;
        // characterMaxMana = 10;
        // characterExpToLevel = 10;
        ButtonListeners();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuActivation();
        }
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
        for (int i = 0; i < skillIcons.Count; i++)
        {
            skillIcons[i].sprite = newIcons[i];
        }
    }

    private void ExperienceBarUpdate(int currExp, float expPercent)
    {
        experienceBarFilling.fillAmount = expPercent;
        experienceText.text = $"{currExp} / {characterExpToLevel}";
    }

    private void ManaBarUpdate(int currMana, float manaPercent)
    {
        manaBarFilling.fillAmount = manaPercent;
        manaText.text = $"{currMana} / {characterMaxMana}";
    }

    private void HealthBarUpdate(int currHp, float hpPercent)
    {
        healthBarFilling.fillAmount = hpPercent;
        healthText.text = $"{currHp} / {characterMaxHp}";
    }

    private void PauseMenuActivation()
    {
        var currentState = pauseMenu.activeInHierarchy;
        pauseMenu.SetActive(!currentState);
        Time.timeScale = !currentState ? 0f : 1f;
    }

    private void CloseMessagePopup()
    {
        messagePopup.SetActive(false);
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