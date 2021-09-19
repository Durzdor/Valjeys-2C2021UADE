using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillController : MonoBehaviour
{
    private const float GlobalCooldown = 0.2f;
    private Character character;
    private bool canUseSkills = true;
    private bool isSkill1Available = true;
    private bool isSkill2Available = true;
    private bool isSkill3Available = true;
    private bool isSkill4Available = true;
    private bool isSkill5Available = true;

    public event Action<float> OnSkill1Use;
    public event Action Skill1;
    public event Action Skill2;
    public event Action Skill3;
    public event Action Skill4;
    public event Action Skill5;
    public event Action<float> OnSkill2Use;
    public event Action<float> OnSkill3Use;
    public event Action<float> OnSkill4Use;
    public event Action<float> OnSkill5Use;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (character.IsAnimationLocked) return;
        if (!canUseSkills) return;
        // starts at 0
        if (character.Input.GetSkillHotkeyInput(0) && isSkill1Available)
        {
            StartCoroutine(GlobalSkillCooldown());
            StartCoroutine(Skill1Cd(character.IsNaomi ? character.Naomi.NaomiSkillData[0].SkillCooldown: character.Ruth.RuthSkillData[0].SkillCooldown));
            print("Skill 1");
            Skill1?.Invoke();
        }

        if (character.Input.GetSkillHotkeyInput(1) && isSkill2Available)
        {
            StartCoroutine(GlobalSkillCooldown());
            StartCoroutine(Skill2Cd(character.IsNaomi ? character.Naomi.NaomiSkillData[1].SkillCooldown: character.Ruth.RuthSkillData[1].SkillCooldown));
            print("Skill 2");
            Skill2?.Invoke();
        }

        if (character.Input.GetSkillHotkeyInput(2) && isSkill3Available)
        {
            StartCoroutine(GlobalSkillCooldown());
            StartCoroutine(Skill3Cd(character.IsNaomi ? character.Naomi.NaomiSkillData[2].SkillCooldown: character.Ruth.RuthSkillData[2].SkillCooldown));
            print("Skill 3");
            Skill3?.Invoke();
        }

        if (character.Input.GetSkillHotkeyInput(3) && isSkill4Available)
        {
            StartCoroutine(GlobalSkillCooldown());
            StartCoroutine(Skill4Cd(character.IsNaomi ? character.Naomi.NaomiSkillData[3].SkillCooldown: character.Ruth.RuthSkillData[3].SkillCooldown));
            print("Skill 4");
            Skill4?.Invoke();
        }

        if (character.Input.GetSkillHotkeyInput(4) && isSkill5Available)
        {
            StartCoroutine(GlobalSkillCooldown());
            StartCoroutine(Skill5Cd(character.IsNaomi ? character.Naomi.NaomiSkillData[4].SkillCooldown: character.Ruth.RuthSkillData[4].SkillCooldown));
            print("Skill 5");
            Skill5?.Invoke();
        }
    }

    private IEnumerator GlobalSkillCooldown()
    {
        canUseSkills = false;
        yield return new WaitForSeconds(GlobalCooldown);
        canUseSkills = true;
    }

    private IEnumerator Skill1Cd(float cooldown)
    {
        isSkill1Available = false;
        var i = cooldown;
        while (i > 0)
        {
            i -= Time.deltaTime;
            OnSkill1Use?.Invoke(i/cooldown);
            yield return null;
        }
        isSkill1Available = true;
    }
    private IEnumerator Skill2Cd(float cooldown)
    {
        isSkill2Available = false;
        var i = cooldown;
        while (i > 0)
        {
            i -= Time.deltaTime;
            OnSkill2Use?.Invoke(i/cooldown);
            yield return null;
        }
        isSkill2Available = true;
    }
    private IEnumerator Skill3Cd(float cooldown)
    {
        isSkill3Available = false;
        var i = cooldown;
        while (i > 0)
        {
            i -= Time.deltaTime;
            OnSkill3Use?.Invoke(i/cooldown);
            yield return null;
        }
        isSkill3Available = true;
    }
    private IEnumerator Skill4Cd(float cooldown)
    {
        isSkill4Available = false;
        var i = cooldown;
        while (i > 0)
        {
            i -= Time.deltaTime;
            OnSkill4Use?.Invoke(i/cooldown);
            yield return null;
        }
        isSkill4Available = true;
    }
    private IEnumerator Skill5Cd(float cooldown)
    {
        isSkill5Available = false;
        var i = cooldown;
        while (i > 0)
        {
            i -= Time.deltaTime;
            OnSkill5Use?.Invoke(i/cooldown);
            yield return null;
        }
        isSkill5Available = true;
    }
}