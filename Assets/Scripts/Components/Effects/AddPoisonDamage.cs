using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Разнести по разным файлам
public class AddPoisonDamage : AddTimedDamage
{
    public override Color GetColor() => Color.green;
    public override float GetAnimationDuration() => .5f;
    public override GameObject AnimationAsset() => GameAssets.SpillingPoison;
}

public class AddBloodDamage : AddTimedDamage
{
    public override Color GetColor() => Color.red;
    public override float GetAnimationDuration() => .5f;
    public override GameObject AnimationAsset() => GameAssets.SpillingBlood;
}

public class AddFireDamage : AddTimedDamage
{
    public readonly Color SalmonOrange = new Color(229/256f, 81/256f, 55/256f);

    public override Color GetColor() => SalmonOrange;

    public override float GetAnimationDuration() => .5f;
    public override GameObject AnimationAsset() => GameAssets.GroundFire;
}
