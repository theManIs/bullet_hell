using System;
using Assets.Scripts.Types;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Scriptables/Perks/New Perk", menuName = "Scriptables/Perk", order = 1), Serializable]
public class PerkScriptableObject : ScriptableObject
{
    public PerkType PerkType;
    public PerkLevel PerkLevel;
    // public float SpeedMultiplier = 1;
    // public float DamageMultiplier = 1;
    // public bool CanMove = true;
    // public float SpeedAddend = 0;
    // public float DamageAddend = 0;
    // public bool CanBeDamaged = true;
    // public bool ApplyOnKill = false;
    // public float MaxDamageMultiplier = 1;
    // public float MaxSpeedMultiplier = 1;

}
