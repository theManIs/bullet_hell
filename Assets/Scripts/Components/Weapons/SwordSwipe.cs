using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwipe : MeleeWeaponFrame
{
    public override WeaponFrame GetAsset() => GameAssets.SwordSwipe;

    public float RepelLevel = .5f;

    public override void Start()
    {
        base.Start();

        Destroy(gameObject, EffectDuration);
    }

    public Vector3 RepelOneStepBack(Transform whomToRepel, Transform pointOfForce, float repelLevel)
    {
        return (whomToRepel.position - pointOfForce.position).normalized * repelLevel;
    }

    public virtual void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (IgnoreEnvPlCl(collider2d.gameObject))
        {
            if (!IgnoreList.Contains(collider2d.gameObject.GetInstanceID()))
            {
                IgnoreList.Add(collider2d.gameObject.GetInstanceID());
                collider2d.gameObject.transform.position += RepelOneStepBack(collider2d.gameObject.transform, TransformOfOrigin, RepelLevel);
            }
        }
    }
}
