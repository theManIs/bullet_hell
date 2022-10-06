using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAdder : MonoBehaviour
{
    public float TriggerEnterLock = .3f;

    // private SpriteRendererEffectAdder srea;
    private EnemyCoxswain _ec;
    private float _lastTrigger;

    public void Awake()
    {
        _ec = GetComponent<EnemyCoxswain>();
        // srea = GetComponent<SpriteRendererEffectAdder>();

        _lastTrigger = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
            if (_ec != null)
            {
                // srea.BlinkOnce();
                _ec.GotHit(collision.gameObject.transform);
                // srea.RepelOneStepBack(collision.gameObject.transform);
            }
        }

        _lastTrigger = Time.time;
    }
}
