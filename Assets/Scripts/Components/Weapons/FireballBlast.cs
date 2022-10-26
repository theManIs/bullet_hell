using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBlast : RangedWeaponFrame
{
    public override WeaponFrame GetAsset() => GameAssets.FireballBlast;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.layer != LayerMask.NameToLayer(PlayerLayer))
        {
            Instantiate(GameAssets.FireBang, transform.position, Quaternion.identity);
        }
    }
}
