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
}
