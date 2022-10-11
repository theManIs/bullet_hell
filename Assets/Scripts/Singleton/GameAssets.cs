using UnityEngine;

public class GameAssets
{
    public static DamageText DamageText => Resources.Load<DamageText>("Effects/DamageText");

    public static CoinOperator CoinOperator => Resources.Load<CoinOperator>("Environment/Coin_1");
    
    public static ArrowShaft ArrowShaft => Resources.Load<ArrowShaft>("Weapons/Arrow");
}
