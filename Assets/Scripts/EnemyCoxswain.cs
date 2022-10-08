using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoxswain : MonoBehaviour
{
    //todo
    public GameObject target;

    [Space]
    // public bool RotateRight = true;
    // public float TiltSpeed = 1f;
    // [Range(0, 360)]
    // public float Tiltmagnitude = 10;
    public float ClosingSpeed = .5f;
    public float ClosingError = 0f;
    public int HealthPoints = 2;
    // public float DisappearTime = 1f;

    // private const int _pi = 180;
    // private const int _2pi = 360;
    private SplitToPixels stp;
    private Quaternion _initRot;
    private HealthSystem _hs;
    private SpriteRendererEffectAdder _srea;
    private Collider2D _c2d;
    private Collider2D _c2dTarget;
    private OnChangeExperience _oce;


    public void Awake()
    {
        stp = GetComponent<SplitToPixels>();
        _hs = new HealthSystem(HealthPoints);
        _srea = GetComponent<SpriteRendererEffectAdder>();
        _c2d = GetComponent<Collider2D>();
        _oce = FindObjectOfType<OnChangeExperience>();

        if (target != null)
        {
            _c2dTarget = target.GetComponent<Collider2D>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _initRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hs.IsDead) return;

        _srea.SwingWhenMoving();
        // if (RotateRight)
        // {
        //     transform.Rotate(0, 0, TiltSpeed * Time.deltaTime);
        //
        //     if (transform.rotation.eulerAngles.z >= Tiltmagnitude && transform.rotation.eulerAngles.z < _pi)
        //     {
        //         RotateRight = false;
        //     }
        // } else
        // {
        //     transform.Rotate(0, 0, -TiltSpeed * Time.deltaTime);
        //
        //     //Debug.Log(transform.rotation.eulerAngles.z);
        //
        //     if (transform.rotation.eulerAngles.z < _2pi - Tiltmagnitude && transform.rotation.eulerAngles.z > _pi)
        //     {
        //         RotateRight = true;
        //     }
        // }

        if (_c2dTarget != null)
        {
            // Collider2D c2d = target.GetComponent<Collider2D>();
            // Collider2D c2dEnemy = GetComponent<Collider2D>();
            bool targetReachedX = Mathf.Abs(target.transform.position.x - transform.position.x) <= _c2dTarget.bounds.extents.x + _c2d.bounds.extents.x + ClosingError;
            bool targetReachedY = Mathf.Abs(target.transform.position.y - transform.position.y) <= _c2dTarget.bounds.extents.y + _c2d.bounds.extents.y + ClosingError;

            //Debug.Log(Mathf.Abs(target.transform.position.x - transform.position.x) + " " + (c2d.bounds.extents.x + c2dEnemy.bounds.extents.x + ClosingError));
            if (!(targetReachedX && targetReachedY))
            {
                //Debug.Log("Target not reached");
                transform.position += (target.transform.position - transform.position).normalized * ClosingSpeed * Time.deltaTime;

            }
            else
            {
                target.GetComponent<KnightCoxswain>().SetDamage();
            }
        }
    }

    public void GotHit(Transform gotHitFrom)
    {
        if (_hs.IsDead) return;

        _hs.ApplyNormalizedDamage();

        if (_hs.IsDead)
        {
            transform.rotation = _initRot;
            _c2d.enabled = false;

            stp.SplitByPixel();
            stp.ToAshes();
            stp.Disappear();
            _oce.InvokeEvent(1);
        }
        else
        {
            _srea.BlinkOnce();
            _srea.RepelOneStepBack(gotHitFrom);
        }
    }
}
