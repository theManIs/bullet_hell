using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAdder : MonoBehaviour
{
    public float TriggerEnterLock = .3f;

    private SpriteRendererEffectAdder srea;
    private EnemyCoxswain ec;
    private float _lastTrigger;

    public void Awake()
    {
        srea = GetComponent<SpriteRendererEffectAdder>();

        _lastTrigger = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        ec = GetComponent<EnemyCoxswain>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        Collider2D[] lc2d = new Collider2D[0];
        ContactFilter2D cf2d = new ContactFilter2D();

        col.OverlapCollider(cf2d.NoFilter(), lc2d);

        if (lc2d.Length > 0)
        {
            Debug.Log(lc2d.Length);
        }
        */

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time - _lastTrigger > TriggerEnterLock)
        {
            if (srea is SpriteRendererEffectAdder)
            {
                // srea.BlinkOnce();
                ec.Die();
                srea.RepelOneStepBack(collision.gameObject.transform);
            }
        }

        _lastTrigger = Time.time;
    }
}
