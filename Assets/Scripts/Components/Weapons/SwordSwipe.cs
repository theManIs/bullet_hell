using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwipe : WeaponFrame
{
    public float EffectDuration = 1f;

    public static SwordSwipe Asset => GameAssets.SwordSwipe;

    public override WeaponFrame GetAsset() => GameAssets.SwordSwipe;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override WeaponFrame Launch(Transform transformOfOrigin)
    {
        TransformOfOrigin = transformOfOrigin;

        InvokeRepeating(nameof(SwipeWithSword), 0, WeaponCooldown);

        return this;
    }

    public void SwipeWithSword()
    {
        SwordSwipe _swordSwipe = Instantiate(Asset, TransformOfOrigin);

        // _swordSwipe.SetActive(true);
        //
        // if (_lastColliderPosition != transform.position)
        // {
        //     _mainCol.offset += Vector2.right * .01f;
        // }

        Destroy(_swordSwipe.gameObject, EffectDuration);
    }

    public void DisableEffect()
    {
        // _swordSwipe.SetActive(false);
        //
        // if (_mainCol.offset != _defaultColliderOffset)
        // {
        //     _mainCol.offset = _defaultColliderOffset;
        // }
    }

}
