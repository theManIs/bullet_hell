using System;
using UnityEngine;

public class LevelingLedger
{
    public const int LevelUpAmount = 1;
    public const int StartLevel = 1;
    public const int StartExperience = 5;
    public const int ExponentialFunction = 2;
    
    public int CurrentLevel;
    public int CurrentExperience;

    public LevelingLedger()
    {
        CurrentLevel = StartLevel;
        CurrentExperience = StartExperience;
    }

    public void LevelUp()
    {
        CurrentLevel += LevelUpAmount;
        CurrentExperience = (int)Math.Pow(LevelUpAmount, ExponentialFunction) + StartExperience;
    }
}
