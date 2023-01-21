using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Texts/New Final Text", menuName = "Scriptables/FinalText", order = 1), Serializable]
public class FinalScreenScriptableObject : ScriptableObject
{
    [SerializeField]
    public Bonus[] Bonuses;

    public string TimeSpent;
    public string KillingMade;
    public string WeaponOneDamage;
    public string WeaponTwoDamage;

    public string SummaryPrompt;
    public string GoldText;

    public string GameOver;
    public string RestartText;
}

//TODO Перенести в отдельный файл
[Serializable]
public struct Bonus
{
    public string BonusTitle;
    public int BonusEffect;
    public bool BonusActive;
}
