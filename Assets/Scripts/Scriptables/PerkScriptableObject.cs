using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Types;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Perks/New Perk", menuName = "Scriptables/Perk", order = 1), Serializable]
public class PerkScriptableObject : ScriptableObject
{
    public Sprite PerkImage;
    public PerkType PerkType;
    public PerkLevel PerkLevel = PerkLevel.Zero;
    public string PerkDescription;
    // public float SpeedMultiplier = 1;
    // public float DamageMultiplier = 1;
    // public bool CanMove = true;
    // public float SpeedAddend = 0;
    // public float DamageAddend = 0;
    // public bool CanBeDamaged = true;
    // public bool ApplyOnKill = false;
    // public float MaxDamageMultiplier = 1;
    // public float MaxSpeedMultiplier = 1;


    public static PerkScriptableObject GetPerkTemplate(PerkType perkType)
    {
        return Instantiate(perkType switch
        {
            PerkType.WeaponCooldown => GameAssets.WeaponCooldown,
            PerkType.WeaponDamage => GameAssets.WeaponDamage,
            PerkType.HealthPoints => GameAssets.HealthPoints,
            PerkType.WeaponRange => GameAssets.WeaponRange,
            PerkType.HealthRegeneration => GameAssets.HealthRegeneration,
            PerkType.WeaponAoe => GameAssets.WeaponAoe,
            _ => GameAssets.HealthPoints
        });
    }

    // public static PerkType RollPerk() => (PerkType) Mathf.Round(Random.value * (int)PerkType.PerkLength - 1);
    public static PerkType RollPerk() => (PerkType) Random.Range(0, 6);

    public static PerkType RollPerkUnique(List<PerkType> lockedPerks)
    {
        PerkType pt = RollPerk();

        return !lockedPerks.Contains(pt) ? pt : RollPerkUnique(lockedPerks);
    }
}
