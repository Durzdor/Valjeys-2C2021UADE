using System;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    #region SerializedFields
#pragma warning disable 649
    [Header("Starting Stats")] [Space(5)]
    [SerializeField] private int startingLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private List<int> expRequirements;
#pragma warning restore 649
    #endregion
    
    private bool _maxLevelReached;
    private float _currMaxExp;

    public event Action OnExpGained;
    public event Action OnLevelUp;
    
    public float CurrentExp { get; private set; }

    public float MaxExp => _maxLevelReached ? float.MaxValue : expRequirements[CurrentLevel];

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
        if (_maxLevelReached) return;
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
           _maxLevelReached = true;
       }
       OnLevelUp?.Invoke();
   }
}