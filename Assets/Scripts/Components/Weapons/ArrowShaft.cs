using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShaft : RangedWeaponFrame
{
    public int PiercingCount = 3; 
    public override WeaponFrame GetAsset() => GameAssets.ArrowShaft; 
    public override void OnTriggerEnter2D(Collider2D collider2d)
    {
       if (IgnoreEnvPlCl(collider2d.gameObject) && PiercingCount <= 0)
       {
           Destroy(gameObject);
       }

       PiercingCount--;
    }
}
