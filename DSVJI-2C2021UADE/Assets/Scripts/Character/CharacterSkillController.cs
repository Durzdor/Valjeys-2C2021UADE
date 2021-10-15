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
    [Header("Error Message")] [Space(5)] 
    [SerializeField] private GameObject errorMessageContainer;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private AudioSource _audioSkill1Melee;
    [SerializeField] private AudioSource _audioSkill1Magic;
    [SerializeField] private AudioSource _audioSkill2Melee;
    [SerializeField] private AudioSource _audioSkill2Magic;

#pragma warning restore 649
    #endregion
    
    private const float GlobalCooldown = 0.2f;
    private Character _character;
    private bool _canUseSkills = true;
    private Skill[] _currentSkillList = new Skill[5];
    // TODO: lista de skills actualmente desbloqueadas por separado de las referencias para saber cuales estan
    private List<Skill> _ruthSkillList;
    private List<Skill> _naomiSkillList;
    private bool _isErrorDisplaying;
    
    private static readonly int Skill1Trigger = Animator.StringToHash("Skill1Trigger");
    private static readonly int Skill2Trigger = Animator.StringToHash("Skill2Trigger");
    private static readonly int Skill3Trigger = Animator.StringToHash("Skill3Trigger");
    private static readonly int Skill4Trigger = Animator.StringToHash("Skill4Trigger");
    private static readonly int Skill5Trigger = Animator.StringToHash("Skill5Trigger");


    private void Awake()
    {
        _character = GetComponent<Character>();
        _ruthSkillList = new List<Skill>(ruthSkillContainer.GetComponents<Skill>());
        _naomiSkillList= new List<Skill>(naomiSkillContainer.GetComponents<Skill>());
    }

    private void Start()
    {
        _character.Animation.OnSwitchComplete += SwapSkillList;
        SwapSkillList();
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
            _currentSkillList[0].UseSkill();
            if (_currentSkillList[0].WasSkillUsed)
            {
                _character.Animator.SetTrigger(Skill1Trigger);
                if (_character.IsNaomi)
                    _audioSkill1Magic.Play();
                else
                    _audioSkill1Melee.Play();
            }
            StartCoroutine(GlobalSkillCooldown());
        }

        if (_character.Input.GetSkill2Input)
        {
            _currentSkillList[1].UseSkill();
            if (_currentSkillList[1].WasSkillUsed)
            {
                _character.Animator.SetTrigger(Skill2Trigger);
                if (_character.IsNaomi)
                    _audioSkill2Magic.Play();
                else
                    _audioSkill2Melee.Play();
            }
            StartCoroutine(GlobalSkillCooldown());
        }

        if (_character.Input.GetSkill3Input)
        {
            _currentSkillList[2].UseSkill();
            if (_currentSkillList[2].WasSkillUsed)
            {
                _character.Animator.SetTrigger(Skill3Trigger);
            }
            StartCoroutine(GlobalSkillCooldown());
        }

        if (_character.Input.GetSkill4Input)
        {
            _currentSkillList[3].UseSkill();
            if (_currentSkillList[3].WasSkillUsed)
            {
                _character.Animator.SetTrigger(Skill4Trigger);
            }
            StartCoroutine(GlobalSkillCooldown());
        }

        if (_character.Input.GetSkill5Input)
        {
            _currentSkillList[4].UseSkill();
            if (_currentSkillList[4].WasSkillUsed)
            {
                _character.Animator.SetTrigger(Skill5Trigger);
            }
            StartCoroutine(GlobalSkillCooldown());
        }
    }
    
    private IEnumerator GlobalSkillCooldown()
    {
        _canUseSkills = false;
        yield return new WaitForSeconds(GlobalCooldown);
        _canUseSkills = true;
    }

    private void SwapSkillList()
    {
        var copiedList = _character.IsNaomi ? _naomiSkillList : _ruthSkillList;
        copiedList.CopyTo(_currentSkillList);
        SkillIconUpdate();
        SkillEventSubscription();
    }

    private void SkillEventSubscription()
    {
        for (int i = 0; i < _currentSkillList.Length; i++)
        {
            var index = i;
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
            var keyBind = "";
            keyBind = newHotkeys[i].ToString();
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
}