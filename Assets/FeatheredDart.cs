using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatheredDart : RangedWeaponFrame
{
    public override WeaponFrame GetAsset() => GameAssets.FeatheredDart;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.layer != LayerMask.NameToLayer(IgnoreDestroyLayer))
       {
           Destroy(gameObject);

            // if (transform.GetChild(0)?.GetComponent<SpriteRenderer>() is { } originSp && GetComponent<Collider2D>() is { } originCol)
            // {
            //     originSp.enabled = false;
            //     originCol.enabled = false;
            // }
            AddPoisonDamage apd = collision.gameObject.GetComponent<AddPoisonDamage>();
            bool added = false;

            if (!(apd is { }))
            {
                apd = collision.gameObject.AddComponent<AddPoisonDamage>();
                added = true;
            }

           if (collision.gameObject.GetComponent<SpriteRendererEffectAdder>() is { } colSp)
           {
               // Debug.Log(colSp.color + " " + colSp.gameObject);

               if (added)
               {
                   apd.InitialColor = colSp.GetInitialColor();
               }

               colSp.SetInitialColor(colSp.GetInitialColor() * Color.green);
               // Debug.Log(colSp.color + " " + colSp.gameObject);
           }
       }
    }
}
