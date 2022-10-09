using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageAdder : MonoBehaviour
{
    public float TriggerEnterLock = .3f;
    public string InteractionLayer = "DamageEnemy";

    // private SpriteRendererEffectAdder srea;
    private EnemyCoxswain _ec;
    private float _lastTrigger;
    private DamageText _dm;

    public void Awake()
    {
        _ec = GetComponent<EnemyCoxswain>();
        _dm = Resources.Load<DamageText>("Effects/DamageText");
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
        // Debug.Log(collision.gameObject.layer + " layer: " + LayerMask.LayerToName(collision.gameObject.layer) + " to layer: " + LayerMask.NameToLayer("DamageEnemy"));
        
        if (collision.gameObject.layer == LayerMask.NameToLayer(InteractionLayer))
        {
            if (Time.time - _lastTrigger > TriggerEnterLock)
            {
                if (_ec != null)
                {
                    Vector3 direction = transform.position - collision.gameObject.transform.position;
                    // srea.BlinkOnce();
                    _ec.GotHit(collision.gameObject.transform);
                    DamageText.Setup(transform.position, Mathf.CeilToInt(Random.value * 2), Random.value > .7f, (int)Mathf.Sign(direction.x));
                    // _dm.Setup(transform.position, Mathf.CeilToInt(Random.value * 2), true);
                    // srea.RepelOneStepBack(collision.gameObject.transform);
                }
            }

            _lastTrigger = Time.time;
        }
    }
}
