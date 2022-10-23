using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwipe : MeleeWeaponFrame
{
    public override WeaponFrame GetAsset() => GameAssets.SwordSwipe;

    public override void Start()
    {
        base.Start();

        Destroy(gameObject, EffectDuration);
    }
    //
    // public override void Setup()
    // {
    //     Instantiate(GetAsset(), TransformOfOrigin);
    // }

}
