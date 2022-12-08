using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class EnemyCoxswain : MonoBehaviour
{
    public event Action<int> AddExperience;

    //todo
    public GameObject target;

    [Space]
    // public bool RotateRight = true;
    // public float TiltSpeed = 1f;
    // [Range(0, 360)]
    // public float Tiltmagnitude = 10;
    public float ClosingSpeed = .5f;
    public float DiffractionSpeed = 1f;
    public float ClosingError = 0f;
    public int HealthPoints = 2;
    public bool SwingInMoving = false;
    public bool DirectionLeft = false;
    public bool LeftAligned = false;
    public Vector3 DirectionToPlayer;
    public float CastDistance = .5f;
    public string ObstacleLayer = "Environment";
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

    public bool IsDead => _hs.IsDead;

    public void Awake()
    {
        stp = GetComponent<SplitToPixels>();
        _hs = new HealthSystem(HealthPoints);
        _srea = GetComponent<SpriteRendererEffectAdder>();
        _c2d = GetComponent<Collider2D>();
        _oce = FindObjectOfType<OnChangeExperience>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(target);
        if (target != null)
        {
            _c2dTarget = target.GetComponent<Collider2D>();
        }

        _initRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hs.IsDead) return;

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

            // Debug.Log(Mathf.Abs(target.transform.position.x - transform.position.x) + " " + (c2d.bounds.extents.x + c2dEnemy.bounds.extents.x + ClosingError));
            if (!(targetReachedX && targetReachedY))
            {
                DirectionToPlayer = target.transform.position - transform.position;
                DirectionLeft = DirectionToPlayer.x < 0;
                _srea.FlipX(LeftAligned ? !DirectionLeft : DirectionLeft);
                // RaycastHit2D rhit = Physics2D.Raycast(_c2d.bounds.center, DirectionToPlayer, _c2d.bounds.size.x + .5f, LayerMask.GetMask("Environment"));
                RaycastHit2D rhit = Physics2D.BoxCast(_c2d.bounds.center, _c2d.bounds.size, 0, DirectionToPlayer, CastDistance, LayerMask.GetMask(ObstacleLayer));
                
                Vector3 mulVector3 = Vector3.zero;

                if (rhit)
                {
                    Collider2D colObs2d = rhit.transform.GetComponent<Collider2D>();
                    // // Vector3 directionE = _c2d.bounds.center - rhit.transform.position;
                    // // Quaternion q = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * directionE);
                    // Vector3 directionToObstacle = colObs2d.bounds.center - _c2d.bounds.center;
                    // float distance = Vector3.Distance(colObs2d.bounds.center, _c2d.bounds.center);
                    // float angleCos = (colObs2d.bounds.center.y - _c2d.bounds.center.y) / distance;
                    // float angle = Mathf.Asin((colObs2d.bounds.center.y - _c2d.bounds.center.y) / distance) * Mathf.Rad2Deg;
                    // mulVector3 = Quaternion.Euler(0, 0, 90) * directionToObstacle;
                    Vector3 directionToObstacle = colObs2d.bounds.center - _c2d.bounds.center;
                    Quaternion q = Quaternion.FromToRotation(directionToObstacle, DirectionToPlayer);


                    mulVector3 = q.eulerAngles.z > Mathf.PI * Mathf.Rad2Deg
                        ? Quaternion.Euler(0, 0, -90) * directionToObstacle
                        : Quaternion.Euler(0, 0, 90) * directionToObstacle;

                    // Debug.DrawRay(_c2d.bounds.center, mulVector3, Color.yellow);
                    // Debug.DrawLine(transform.position, colObs2d.bounds.center, Color.green);
                    // Debug.Log(rhit.transform.gameObject.name + " " +  gameObject.name + " " + q.eulerAngles + " " + mulVector3);
                }

                //Debug.Log("Target not reached");
                transform.position += mulVector3 != Vector3.zero 
                    ? mulVector3.normalized * (rhit ? DiffractionSpeed : ClosingSpeed) * Time.deltaTime
                    : (target.transform.position - transform.position).normalized * ClosingSpeed * Time.deltaTime;

                if (SwingInMoving)
                {
                    _srea.SwingWhenMoving();
                }
            }
            else
            {
                target.GetComponent<KnightCoxswain>().SetDamage();
            }
        }
    }

    public void OnEnable() => AddExperience += _oce.AddExperience;
    public void OnDisable() => AddExperience -= _oce.AddExperience;

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
            AddExperience?.Invoke(1);
            DropCoin();
        }
        else
        {
            _srea.BlinkOnce();
            // _srea.RepelOneStepBack(gotHitFrom);
        }
    }

    public void DropCoin() => CoinOperator.Setup(transform.position);
}
