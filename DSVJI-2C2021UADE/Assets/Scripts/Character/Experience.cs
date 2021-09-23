using System;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [Header("Starting Stats")] [Space(5)]
    [SerializeField] private int startingLevel = 0;
    [SerializeField] private int maxLevel = 0;
    [SerializeField] private List<int> expRequirements = null;
    
    private bool maxLevelReached;
    private float currMaxExp;

    public event Action OnExpGained;
    public event Action OnLevelUp;
    
    public float CurrentExp { get; private set; }

    public float MaxExp => maxLevelReached ? float.MaxValue : expRequirements[CurrentLevel];

    public int CurrentLevel { get; private set; }
    public int MaxLevel => maxLevel;
    
    public float GetRatio => CurrentExp / MaxExp;

    private void Awake()
    {
        CurrentLevel = startingLevel;
        //handle level up for each level gained
    }

    public void GainExp(float expAmount)
    {
        if (maxLevelReached) return;
        var expBefore = CurrentExp;
        CurrentExp += expAmount;
        CurrentExp = Mathf.Clamp(CurrentExp, 0, MaxExp);
        HandleLevelUp();
        // call OnExpGained action
        var trueExpAmount = CurrentExp - expBefore;
        HandleExpOverflow(expAmount - trueExpAmount);
        if (trueExpAmount > 0f)
        {
            // use this to display amount healed on screen
            OnExpGained?.Invoke();
        }
    }

    private void HandleExpOverflow(float extraExp)
    {
        GainExp(extraExp);
    }
   private void HandleLevelUp()
   {
       // call OnLevelUp action
       if (!(CurrentExp >= MaxExp)) return;
       CurrentLevel++;
       CurrentExp = 0;
       if (CurrentLevel > MaxLevel)
       {
           CurrentLevel = MaxLevel;
           maxLevelReached = true;
       }
       OnLevelUp?.Invoke();
   }
}