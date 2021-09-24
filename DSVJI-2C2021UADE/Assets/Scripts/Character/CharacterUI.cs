﻿using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    #region SerializedFields

#pragma warning disable 649
    [Header("GameObjects to activate")] [Space(5)]
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
    
    private Character _character;
    private float _characterMaxHp;
    private float _characterMaxMana;
    private float _characterExpToLevel;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private void Start()
    {
        FirstLoad();
        CharacterEventSubscriptions();
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
        _characterMaxHp = _character.Health.MaxHealth;
        _characterMaxMana = _character.Mana.MaxMana;
        _characterExpToLevel = _character.Experience.MaxExp;
        HealthBarUpdate(_character.Health.CurrentHealth, _character.Health.GetRatio);
        ManaBarUpdate(_character.Mana.CurrentMana, _character.Mana.GetRatio);
        ExperienceBarUpdate(_character.Experience.CurrentExp, _character.Experience.GetRatio);
        NameChange(_character.IsNaomi ? "Naomi" : "Ruth");
        LevelUpUpdate();
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
        _character.Health.OnDamaged += delegate
        {
            DamageTakenVFX();
            HealthBarUpdate(_character.Health.CurrentHealth, _character.Health.GetRatio);
        };
        _character.Health.OnHealed += delegate
        {
            HealthBarUpdate(_character.Health.CurrentHealth, _character.Health.GetRatio);
        };
        _character.Mana.OnConsumed += delegate { ManaBarUpdate(_character.Mana.CurrentMana, _character.Mana.GetRatio); };
        _character.Mana.OnGained += delegate { ManaBarUpdate(_character.Mana.CurrentMana, _character.Mana.GetRatio); };
        _character.Ruth.OnRuthEnable += delegate
        {
            NameChange("Ruth");
        };
        _character.Naomi.OnNaomiEnable += delegate
        {
            NameChange("Naomi");
        };
        _character.Experience.OnExpGained += delegate
        {
            ExperienceBarUpdate(_character.Experience.CurrentExp, _character.Experience.GetRatio);
        };
        _character.Experience.OnLevelUp += LevelUpUpdate;
        _character.OnCharacterInteractRange += InteractTextHandler;
        _character.OnCharacterCheckpointUsed += CheckpointFeedback;
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
        yield return new WaitForSeconds(0.1f);
        damageTakenPlateVFX.gameObject.SetActive(false);
    }
    
    private void ExperienceBarUpdate(float currExp, float expPercent)
    {
        experienceBarFilling.fillAmount = expPercent;
        experienceText.text = $"{currExp} / {_characterExpToLevel}";
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
}