using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBang : MonoBehaviour
{
    public float EffectDuration = 2f;

    public void Start()
    {
        Destroy(gameObject, EffectDuration);
    }


}
