using UnityEngine;

public class GameAssets
{
    public static DamageText DamageText => Resources.Load<DamageText>("Effects/DamageText");
    public static FireBang FireBang => Resources.Load<FireBang>("Effects/FireBang");
    public static CoinOperator CoinOperator => Resources.Load<CoinOperator>("Environment/Coin_2");
    public static ArrowShaft ArrowShaft => Resources.Load<ArrowShaft>("Weapons/Arrow");
    public static SwordSwipe SwordSwipe => Resources.Load<SwordSwipe>("Weapons/Swipe");
    public static FireballBlast FireballBlast => Resources.Load<FireballBlast>("Weapons/Fireball");
    public static FeatheredDart FeatheredDart => Resources.Load<FeatheredDart>("Weapons/Dart");
    public static QiangPoke QiangPoke => Resources.Load<QiangPoke>("Weapons/Qiang");
    public static ReaperScythe ReaperScythe => Resources.Load<ReaperScythe>("Weapons/Scythe");
    public static ChakramOne ChakramOne => Resources.Load<ChakramOne>("Weapons/Chakram");
    public static GuardianShield GuardianShield => Resources.Load<GuardianShield>("Weapons/Shield");

    public static GameObject SpillingBlood => Resources.Load<GameObject>("Effects/SpillingBlood");
    public static GameObject SpillingPoison => Resources.Load<GameObject>("Effects/SpillingPoison");
    public static GameObject GroundFire => Resources.Load<GameObject>("Effects/GroundFire");

    public static PerkScriptableObject HealthPoints => Resources.Load<PerkScriptableObject>("Scriptables/Perks/HealthPoints");
    public static PerkScriptableObject HealthRegeneration => Resources.Load<PerkScriptableObject>("Scriptables/Perks/HealthRegeneration");
    public static PerkScriptableObject WeaponCooldown => Resources.Load<PerkScriptableObject>("Scriptables/Perks/WeaponCooldown");
    public static PerkScriptableObject WeaponDamage => Resources.Load<PerkScriptableObject>("Scriptables/Perks/WeaponDamage");
    public static PerkScriptableObject WeaponRange => Resources.Load<PerkScriptableObject>("Scriptables/Perks/WeaponRange");
    public static PerkScriptableObject WeaponAoe => Resources.Load<PerkScriptableObject>("Scriptables/Perks/WeaponAoe");
    public static ScriptableTile MyFirstScriptableTile => Resources.Load<ScriptableTile>("Scriptables/Tiles/MyFirstScriptableTile");
    public static ScriptableTileObstacle ObstacleTile => Resources.Load<ScriptableTileObstacle>("Scriptables/Tiles/ObstacleTile");
}
