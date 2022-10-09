using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class KnightCoxswain : MonoBehaviour, CoinTaker
{
    //todo
    public GameObject SwordSwipeGo;
    public float SwipeFrequency = 1f;
    public float EffectDuration = 1f;
    public HealthBarsSet HealthBar;

    [Range(0, 10)]
    public float MoveSpeed = 2;
    [Range(1, 100)]
    public int HealthPoints = 5;
    [Range(0, 10)]
    public float InvulnerabilityTime = 1f;

    private GameObject _swordSwipe;
    private SpriteRenderer _knightSp;
    private SpriteRenderer _swipeSp;
    private Collider2D _mainCol;
    private Vector3 _lastColliderPosition;
    private Vector2 _defaultColliderOffset;
    private HealthSystem _hs;
    private SpriteRendererEffectAdder _srea;
    private DisplayControl _dc;
    private RectangleBar _healthBar;

    public void Awake()
    {
        _mainCol = GetComponent<Collider2D>();
        _lastColliderPosition = transform.position;
        _defaultColliderOffset = _mainCol.offset;
        _hs = new HealthSystem(HealthPoints) { InvulnerabilityTime = InvulnerabilityTime };
        _srea = GetComponent<SpriteRendererEffectAdder>();
        _dc = FindObjectOfType<DisplayControl>();
        _knightSp = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _healthBar = HealthBar switch
        {
            HealthBarsSet.HealthBarCut => _dc.HealthBars.HealthBarCut,
            HealthBarsSet.HealthBarShrink => _dc.HealthBars.HealthBarShrink,
            HealthBarsSet.HealthBarFade => _dc.HealthBars.HealthBarFade,
            _ => _healthBar
        };
        _healthBar.gameObject.SetActive(true);
        // _dc.HealthBars.HealthBarShrink.ResetSystem(HealthPoints);

        if (SwordSwipeGo)
        {
            _swordSwipe = Instantiate(SwordSwipeGo, transform);
            _swordSwipe.SetActive(false);
            _swipeSp = _swordSwipe.GetComponent<SpriteRenderer>();

            InvokeRepeating(nameof(SwipeWithSword), SwipeFrequency, SwipeFrequency);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

//        Debug.Log(xAxis + " " + yAxis);

        if (!xAxis.Equals(0) || !yAxis.Equals(0))
        {
            transform.position += Vector3.right * Time.deltaTime * MoveSpeed * xAxis + Vector3.up * Time.deltaTime * MoveSpeed * yAxis;

            _knightSp.flipX = !(xAxis > 0);
            _swipeSp.flipX = !(xAxis > 0);

            _srea.Move();
        }
        else
        {
            _srea.Stop();
        }
    }

    public void OnEnable() => _hs.OnHealthChanged += WhenGetHit;

    public void OnDisable() => _hs.OnHealthChanged -= WhenGetHit;

    public void SwipeWithSword()
    {
        if (SwordSwipeGo)
        {
            _swordSwipe.SetActive(true);

            if (_lastColliderPosition != transform.position)
            {
                _mainCol.offset += Vector2.right * .01f;
            }

            Invoke(nameof(DisableEffect), EffectDuration);
        }
    }

    public void DisableEffect()
    {
        _swordSwipe.SetActive(false);

        if (_mainCol.offset != _defaultColliderOffset)
        {
            _mainCol.offset = _defaultColliderOffset;
        }
    }

    public void SetDamage()
    {
        // int damage = 

        if (_hs != null)
        {
            _hs.ApplyNormalizedDamage();
        }

        // if (damage > 0 && _srea != null)
        // {
        //     _srea.BlinkOnce();
        //     _dc.HealthBars.HealthBarShrink.SetDamage((int)((float)damage / HealthPoints * 100));
        //     // Debug.Log((float)damage / HealthPoints + " " + ((float)damage / HealthPoints) * 100 + " " + (int)((float)damage / HealthPoints * 100));
        // }

        // Debug.Log("He has health: " + _hs.GetHealth +  " He is dead: " + _hs.IsDead);
        // if (_hs.IsDead)
        // {
        //     Destroy(gameObject, 0f);
        // }
    }

    public void WhenGetHit(object sender, EventArgs args)
    {
        // Debug.Log("Hit!");
        if (args is HealthSystemEventArguments hsea)
        {
            // Debug.Log(hsea.EventType);
            if (hsea.EventType == EventTypeSet.Damage)
            {
                // Debug.Log(hsea.DamageAmount);
                _srea.BlinkOnce();
                // _healthBar.SetDamage((int)((float)hsea.DamageAmount / HealthPoints * 100));
                _healthBar.SetDamage(hsea.DamageAmount);

                if (_hs.IsDead)
                {
                    Destroy(gameObject, 0f);
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        // if (collider2D.GetComponent<CoinOperator>())
        // {
        //     // Destroy(collider2D.gameObject);
        // }
    }
}
