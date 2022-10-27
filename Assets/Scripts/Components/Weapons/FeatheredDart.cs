using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatheredDart : RangedWeaponFrame
{
    public override WeaponFrame GetAsset() => GameAssets.FeatheredDart;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
       if (IgnoreEnvPlCl(collision.gameObject))
       {
           Destroy(gameObject);

            // bool destroyed = false;
            //  Color oldInitColor = Color.white;
            //  
            //  if (collision.gameObject.GetComponent<AddPoisonDamage>() is { } apd)
            //  {
            //      Destroy(apd);
            //      destroyed = true;
            //      oldInitColor = apd.InitialColor;
            //  }
            //
            //  AddPoisonDamage apdNew = collision.gameObject.AddComponent<AddPoisonDamage>();
            //
            // if (collision.gameObject.GetComponent<SpriteRendererEffectAdder>() is { } colSp)
            // {
            //     // Debug.Log(colSp.color + " " + colSp.gameObject);
            //
            //     apdNew.InitialColor = !destroyed ? colSp.GetInitialColor() : oldInitColor;
            //     colSp.SetInitialColor(colSp.GetInitialColor() * Color.green);
            //     // Debug.Log(colSp.color + " " + colSp.gameObject);
            // }

            // if (collision.gameObject.GetComponent<SpriteRendererEffectAdder>() is { } colSp)
            // {
            //     if (collision.gameObject.GetComponent<AddPoisonDamage>() is { } apd)
            //     {
            //         apd.ReinitializePoison(colSp.GetInitialColor());
            //     }
            //     else
            //     {
            //         AddPoisonDamage apdNew = collision.gameObject.AddComponent<AddPoisonDamage>();
            //
            //         apdNew.InitialColor = colSp.GetInitialColor();
            //     }
            // }

            AddTimedDamage.Setup<AddPoisonDamage>(collision.gameObject);
       }
    }
}
