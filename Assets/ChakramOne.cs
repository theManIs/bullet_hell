using System.Collections.Generic;
using UnityEngine;

public class ChakramOne : RangedWeaponFrame
{
    public int RecursionDepth = 1;
    public List<int> IgnoreList;
    public float BounceLength = .5f;

    public override WeaponFrame GetAsset() => GameAssets.ChakramOne;

    public override void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (IgnoreEnvPl(collider2d.gameObject))
        {
            if (!IgnoreList.Contains(collider2d.gameObject.GetInstanceID()))
            {
                Destroy(gameObject);

                if (RecursionDepth > 0)
                {
                    IgnoreList.Add(collider2d.gameObject.GetInstanceID());
                    // List<Vector3> v3s = new List<Vector3>(FetchVector3FromEnemyCoxswain(collider2d.gameObject.GetInstanceID()));

                    if (FindEnemyPosition(BounceLength * WeaponRange, IgnoreList) != Vector3.zero)
                    {
                        if (Setup(Instantiate(GetAsset(), transform.position, Quaternion.identity)) is ChakramOne cOne)
                        {
                            cOne.EnemyPosition = cOne.FindEnemyPosition(BounceLength * WeaponRange, IgnoreList);
                            cOne.RecursionDepth = RecursionDepth - 1;
                            cOne.IgnoreList = IgnoreList;
                        }
                    }
                }
            }
        }
    }
}
