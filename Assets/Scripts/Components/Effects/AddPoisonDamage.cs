using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPoisonDamage : MonoBehaviour
{
    public Color InitialColor;
    public Color GreenColor = Color.green;
    public float EffectCooldown = 1f;
    public float EffectDuration = 5f;
    public void Start()
    {
        InvokeRepeating(nameof(AddDamage), EffectCooldown, EffectCooldown);
        Invoke(nameof(EndDuration), EffectDuration);
    }


    public void AddDamage()
    {
        if (GetComponent<EnemyCoxswain>() is { } eCox)
        {
            eCox.GotHit(transform);

            DamageText.Setup(transform.position, Mathf.CeilToInt(Random.value * 2), Random.value > .7f, (int)Mathf.Sign(Random.value -.5f));

            // if (eCox.IsDead)
            // {
                // Destroy(gameObject);
            // }
        }
    }

    public void EndDuration()
    {
        if (GetComponent<SpriteRendererEffectAdder>() is { } colSp)
        {
            colSp.SetInitialColor(InitialColor);
        }

        Destroy(this);
    }

    public static void Setup(GameObject gm)
    {
        if (gm.GetComponent<SpriteRendererEffectAdder>() is { } colSp)
        {
            if (gm.GetComponent<AddPoisonDamage>() is { } apd)
            {
                AddPoisonDamage apdNew = gm.AddComponent<AddPoisonDamage>();

                apdNew.InitialColor = apd.InitialColor;
                colSp.SetInitialColor(apdNew.GreenColor);

                Destroy(apd);
            }
            else
            {
                AddPoisonDamage apdNew = gm.AddComponent<AddPoisonDamage>();

                apdNew.InitialColor = colSp.GetInitialColor();
                colSp.SetInitialColor(apdNew.GreenColor);
            }
        }
    }
}
