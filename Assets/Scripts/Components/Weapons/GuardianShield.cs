using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianShield : WeaponFrame
{
    public int ChargeCount = 1;
    public float InvulnerabilityTime = .5f;

    public override WeaponFrame GetAsset() => GameAssets.GuardianShield;

    public override void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(PlayerLayer);
        // GetComponent<Collider2D>().isTrigger = true;
        // DirectionVector3 = (EnemyPosition - transform.position).normalized;
        BoxCollider2D shieldBc2d = GetComponent<BoxCollider2D>();
        BoxCollider2D playerBc2d = TransformOfOrigin.GetComponent<BoxCollider2D>();
        shieldBc2d.offset = playerBc2d.offset;
        shieldBc2d.size = playerBc2d.size;
        playerBc2d.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (IgnoreEnvironmentL(collider2d.gameObject))
        {
            ChargeCount--;

            if (ChargeCount <= 0)
            {
                Invoke(nameof(DestroyAfter), InvulnerabilityTime);
            }
        }
    }

    public void DestroyAfter()
    {
        Destroy(gameObject);
        TransformOfOrigin.GetComponent<BoxCollider2D>().enabled = true;
    }

    public override WeaponFrame Setup()
    {
        if (!TransformOfOrigin.GetComponentInChildren<GuardianShield>())
        {
            return base.Setup();
        }

        return default;
    }
}
