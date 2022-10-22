using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponFrame : WeaponFrame
{
    public float EffectDuration = 1f;
    
    public override WeaponFrame Launch(Transform transformOfOrigin)
    {
        TransformOfOrigin = transformOfOrigin;

        InvokeRepeating(nameof(Setup), 0, WeaponCooldown);

        return this;
    }

    public virtual void Setup()
    {
        WeaponFrame swordSwipe = Instantiate(GetAsset(), TransformOfOrigin);

        Destroy(swordSwipe.gameObject, EffectDuration);
    }
}
