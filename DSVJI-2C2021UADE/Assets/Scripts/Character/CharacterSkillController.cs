using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkillController : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Skill Containers")] [Space(5)] 
    [SerializeField] private GameObject ruthSkillContainer;
    [SerializeField] private GameObject naomiSkillContainer;
    [Header("Skill Lists")] [Space(5)] 
    [SerializeField] private List<Image> skillIcons;
    [SerializeField] private List<Image> skillCooldownFill;
    [SerializeField] private List<TextMeshProUGUI> skillHotkeys;
    [SerializeField] private List<bool> naomiUnlockedSkillList;
    [SerializeField] private List<bool> ruthUnlockedSkillList;
    [Header("Skill Misc")] [Space(5)] 
    [SerializeField] private Sprite lockedSkillSprite;
    [Header("Error Message")] [Space(5)] 
    [SerializeField] private GameObject errorMessageContainer;
    [SerializeField] private TextMeshProUGUI errorText;
#pragma warning restore 649
    #endregion
    
    private const float GLOBAL_COOLDOWN = 0.2f;
    private Character _character;
    private bool _canUseSkills = true;
    private Skill[] _currentSkillList = new Skill[5];
    private List<Skill> _ruthSkillList;
    private List<Skill> _naomiSkillList;
    private bool _isErrorDisplaying;
    
    public event Action OnSkill1;
    public event Action OnSkill2;
    public event Action OnSkill3;
    public event Action OnSkill4;
    public event Action OnSkill5;

    public List<bool> NaomiUnlockedSkillList => naomiUnlockedSkillList;
    public List<bool> RuthUnlockedSkillList => ruthUnlockedSkillList;
    
    private void Awake()
    {
        _character = GetComponent<Character>();
        _ruthSkillList = new List<Skill>(ruthSkillContainer.GetComponents<Skill>());
        _naomiSkillList= new List<Skill>(naomiSkillContainer.GetComponents<Skill>());
    }

    private void Start()
    {
        _character.Animation.OnSwitchComplete += UpdateSkillList;
        UpdateSkillList();
        CooldownFirstLoad();
        SkillHotkeysDisplay(_character.Input.skillKeyCodes);
    }

    private void Update()
    {
        if (_character.IsAnimationLocked) return;
        if (!_canUseSkills) return;
        // starts at 0
        if (_character.Input.GetSkill1Input)
        {
            if (_currentSkillList[0] == null) return;
            _currentSkillList[0].UseSkill();
            if (_currentSkillList[0].WasSkillUsed)
            {
                OnSkill1?.Invoke();
            }
            StartCoroutine(GlobalSkillCooldown());
        }

        if (_character.Input.GetSkill2Input)
        {
            if (_currentSkillList[1] == null) return;
            _currentSkillList[1].UseSkill();
            if (_currentSkillList[1].WasSkillUsed)
            {
                OnSkill2?.Invoke();
            }
            StartCoroutine(GlobalSkillCooldown());
        }

        if (_character.Input.GetSkill3Input)
        {
            if (_currentSkillList[2] == null) return;
            _currentSkillList[2].UseSkill();
            if (_currentSkillList[2].WasSkillUsed)
            {
                OnSkill3?.Invoke();
            }
            StartCoroutine(GlobalSkillCooldown());
        }

        if (_character.Input.GetSkill4Input)
        {
            if (_currentSkillList[3] == null) return;
            _currentSkillList[3].UseSkill();
            if (_currentSkillList[3].WasSkillUsed)
            {
                OnSkill4?.Invoke();
            }
            StartCoroutine(GlobalSkillCooldown());
        }

        if (_character.Input.GetSkill5Input)
        {
            if (_currentSkillList[4] == null) return;
            _currentSkillList[4].UseSkill();
            if (_currentSkillList[4].WasSkillUsed)
            {
                OnSkill5?.Invoke();
            }
            StartCoroutine(GlobalSkillCooldown());
        }
    }
    
    private IEnumerator GlobalSkillCooldown()
    {
        _canUseSkills = false;
        yield return new WaitForSeconds(GLOBAL_COOLDOWN);
        _canUseSkills = true;
    }

    private void UpdateSkillList()
    {
        var copiedList = _character.IsNaomi ? _naomiSkillList : _ruthSkillList;
        copiedList.CopyTo(_currentSkillList);
        var unlockList = _character.IsNaomi ? naomiUnlockedSkillList : ruthUnlockedSkillList;
        for (int i = 0; i < _currentSkillList.Length; i++)
        {
            if (unlockList[i] == false)
            {
                _currentSkillList[i] = null;
            }
        }
        SkillIconUpdate();
        SkillEventSubscription();
    }

    private void SkillEventSubscription()
    {
        for (int i = 0; i < _currentSkillList.Length; i++)
        {
            var index = i;
            if (_currentSkillList[index] == null) continue;
            _currentSkillList[index].OnCooldownUpdate += delegate(float cooldownRatio) { SkillCooldownUpdate(skillCooldownFill[index],cooldownRatio); };
            _currentSkillList[index].OnSkillNotUsable += ErrorMessageDisplay;
        }
    }

    private void CooldownFirstLoad()
    {
        foreach (var t in skillCooldownFill)
        {
            SkillCooldownUpdate(t, 0);
        }
    }
    
    private void SkillCooldownUpdate(Image cooldownFill, float cooldownRatio)
    {
        cooldownFill.fillAmount = cooldownRatio;
    }

    private void SkillHotkeysDisplay(List<KeyCode> newHotkeys)
    {
        for (int i = 0; i < skillHotkeys.Count; i++)
        {
            var keyBind = newHotkeys[i].ToString();
            if (newHotkeys[i] == KeyCode.Mouse0)
            {
                keyBind = "MB0";
            }

            if (newHotkeys[i] == KeyCode.Mouse1)
            {
                keyBind = "MB1";
            }
            
            skillHotkeys[i].text = keyBind;
        }
    }
    
    private void SkillIconUpdate()
    {
        for (int i = 0; i < skillIcons.Count; i++)
        {
            if (_currentSkillList[i] == null)
            {
                skillIcons[i].sprite = lockedSkillSprite;
                continue;
            }
            skillIcons[i].sprite = _currentSkillList[i].SkillData.Image;
        }
    }

    private void ErrorMessageDisplay(string message)
    {
        if (!_isErrorDisplaying)
        {
            StartCoroutine(TextFade(message));
        }
    }

    private IEnumerator TextFade(string message)
    {
        _isErrorDisplaying = true;
        errorText.text = message;
        errorMessageContainer.SetActive(true);
        var timeElapsed = 0f;
        var lerpDuration = 1f;
        var prevColor = errorText.color;
        while (timeElapsed < lerpDuration)
        {
            timeElapsed += Time.deltaTime;
            var lerp = Color.Lerp(errorText.color,Color.clear, Time.deltaTime);
            errorText.color = lerp;
            yield return null;
        }
        errorMessageContainer.SetActive(false);
        errorText.color = prevColor;
        _isErrorDisplaying = false;
    }

    public void UnlockNaomiSkill(int skillIndex)
    {
        naomiUnlockedSkillList[skillIndex] = true;
        UpdateSkillList();
    }
    public void UnlockRuthSkill(int skillIndex)
    {
        ruthUnlockedSkillList[skillIndex] = true;
        UpdateSkillList();
    }
}