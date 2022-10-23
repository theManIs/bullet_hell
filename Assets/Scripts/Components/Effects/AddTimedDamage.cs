using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AddTimedDamage : MonoBehaviour
{
    public Color InitialColor;
    public float EffectCooldown = 1f;
    public float EffectDuration = 5f;
    public bool TheFirstImpose = false;

    public void Start()
    {
        if (GetComponent<SpriteRendererEffectAdder>() is { } colSp)
        {
            colSp.SetInitialColor(GetColor());
        }

        if (AnimationAsset() && TheFirstImpose)
        {
            Destroy(Instantiate(AnimationAsset(), transform), GetAnimationDuration());
        }

        InvokeRepeating(nameof(AddDamage), EffectCooldown, EffectCooldown);
        Invoke(nameof(EndDuration), EffectDuration);
    }

    public abstract Color GetColor();
    public abstract float GetAnimationDuration();

    public void AddDamage()
    {
        if (GetComponent<EnemyCoxswain>() is { } eCox)
        {
            eCox.GotHit(transform);

            DamageText.Setup(transform.position, Mathf.CeilToInt(Random.value * 2), Random.value > .7f, (int)Mathf.Sign(Random.value - .5f));

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

    public abstract GameObject AnimationAsset();

    public static AddTimedDamage Setup<T>(GameObject gm) where T : AddTimedDamage
    {
        T apdNew = default;

        if (gm.GetComponent<SpriteRendererEffectAdder>() is { } colSp)
        {
            if (gm.GetComponent<T>() is { } apd)
            {
                apdNew = gm.AddComponent<T>();
                apdNew.InitialColor = apd.InitialColor;

                Destroy(apd);
            }
            else
            {
                apdNew = gm.AddComponent<T>();
                apdNew.InitialColor = colSp.GetInitialColor();
                apdNew.TheFirstImpose = true;
            }
        }

        return apdNew;
    }
}
