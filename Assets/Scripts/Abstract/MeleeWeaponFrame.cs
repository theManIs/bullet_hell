using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponFrame : WeaponFrame
{
    public float EffectDuration = 1f;

    // public override WeaponFrame Launch(Transform transformOfOrigin)
    // {
    //     TransformOfOrigin = transformOfOrigin;
    //
    //     InvokeRepeating(nameof(Setup), 0, WeaponCooldown);
    //
    //     return this;
    // }
    //
    // public virtual void Setup()
    // {
    //     if (Instantiate(GetAsset(), TransformOfOrigin) is MeleeWeaponFrame rs)
    //     {
    //         rs.TransformOfOrigin = TransformOfOrigin;
    //         rs.LastAxis = TransformOfOrigin.GetComponent<KnightCoxswain>().LastDirectionLeft ? 1 : -1;
    //         rs.EnemyPosition = PickEnemy(FetchVector3FromEnemyCoxswain(), TransformOfOrigin.position);
    //     }
    // }

    public void DestroyIfNoEnemy()
    {
        if (EnemyPosition == Vector3.zero)
        {
            Destroy(gameObject);
        }
    }
}
