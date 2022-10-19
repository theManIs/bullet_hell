using UnityEngine;

public class GameAssets
{
    public static DamageText DamageText => Resources.Load<DamageText>("Effects/DamageText");

    public static CoinOperator CoinOperator => Resources.Load<CoinOperator>("Environment/Coin_2");
    
    public static ArrowShaft ArrowShaft => Resources.Load<ArrowShaft>("Weapons/Arrow");

    public static SwordSwipe SwordSwipe => Resources.Load<SwordSwipe>("Weapons/Swipe");

    public static FireballBlast FireballBlast => Resources.Load<FireballBlast>("Weapons/Fireball");
}
