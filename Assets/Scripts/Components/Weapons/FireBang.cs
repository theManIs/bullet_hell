using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBang : MonoBehaviour
{
    public float EffectDuration = 2f;
    public string PlayerLayer = "Player";

    public void Start()
    {
        Destroy(gameObject, EffectDuration);
    }
    
    public void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (collider2d.gameObject.layer != LayerMask.NameToLayer(PlayerLayer))
        {
            AddTimedDamage.Setup<AddFireDamage>(collider2d.gameObject);
        }
    }
}
