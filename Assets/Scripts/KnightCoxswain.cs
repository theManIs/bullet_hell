using System;
using System.Collections.Generic;
using System.Reflection;
using Assets.Scripts.Types;
using UnityEngine;
using UnityEngine.UI;

//TODO Перевести использование гейобжекта на скриптабл обжект
public class KnightCoxswain : MonoBehaviour
{   
    // public GameObject SwordSwipeGo;
    // public float SwipeFrequency = 1f;
    // public float EffectDuration = 1f;
    public HealthBarsSet HealthBar;
    public WeaponsSet RightHand;
    public bool LastDirectionLeft = false;
    public PerkProcessor PerkProcessor;

    [Range(0, 10)]
    public float MoveSpeed = 2;
    [Range(1, 100)]
    public int HealthPoints = 5;
    [Range(0, 10)]
    public float InvulnerabilityTime = 1f;

    // private GameObject _swordSwipe;
    private SpriteRenderer _knightSp;
    private SpriteRenderer _swipeSp;
    private Collider2D _mainCol;
    // private Vector3 _lastColliderPosition;
    // private Vector2 _defaultColliderOffset;
    private HealthSystem _hs;
    private SpriteRendererEffectAdder _srea;
    private DisplayControl _dc;
    private RectangleBar _healthBar;
    private WeaponFrame _wp;
    private ExperienceBarFade _ebf;
    private GameObject _wings;

    public void Awake()
    {
        _mainCol = GetComponent<Collider2D>();
        // _lastColliderPosition = transform.position;
        // _defaultColliderOffset = _mainCol.offset;
        _hs = new HealthSystem(HealthPoints) { InvulnerabilityTime = InvulnerabilityTime };
        _srea = GetComponent<SpriteRendererEffectAdder>();
        _dc = FindObjectOfType<DisplayControl>();
        _knightSp = GetComponent<SpriteRenderer>();
        PerkProcessor = new PerkProcessor().Subscribe(FindObjectOfType<PickingPerkPanel>());
        _ebf = FindObjectOfType<ExperienceBarFade>();
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

        // if (SwordSwipeGo)
        // {
        //     _swordSwipe = Instantiate(SwordSwipeGo, transform);
        //     _swordSwipe.SetActive(false);
        //     _swipeSp = _swordSwipe.GetComponent<SpriteRenderer>();

        _wp = RightHand switch
        {
            WeaponsSet.SwordSwipe => GameAssets.SwordSwipe,
            WeaponsSet.ArrowShaft => GameAssets.ArrowShaft,
            WeaponsSet.FireballBlast => GameAssets.FireballBlast,
            WeaponsSet.FeatheredDart => GameAssets.FeatheredDart,
            WeaponsSet.QiangPoke => GameAssets.QiangPoke,
            WeaponsSet.ReaperScythe => GameAssets.ReaperScythe,
            WeaponsSet.ChakramOne => GameAssets.ChakramOne,
            WeaponsSet.GuardianShield => GameAssets.GuardianShield,
            _ => _wp
        };

        _wp.Launch(transform);
            // _wp = SwordSwipe.Asset.Launch(transform);
            


            // InvokeRepeating(nameof(GoWeaponGo), 0, 2f);
            //InvokeRepeating(nameof(SwipeWithSword), SwipeFrequency, SwipeFrequency);
        // }

        transform.position = Vector3.zero;
    }

    public void OnDrawGizmos()
    {
        // Gizmos.color = Color.yellow;
        if (_wp)
        {
            Gizmos.DrawWireSphere(transform.position, _wp.WeaponRange);
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
            LastDirectionLeft = !(xAxis > 0); 

            if (_swipeSp)
            {
                _swipeSp.flipX = !(xAxis > 0);
            }

            _srea.Move();
        }
        else
        {
            _srea.Stop();
        }
    }

    public void CreateLevelUpWings()
    {
        _wings = Instantiate(GameAssets.LevelUp, transform);
        _wings.transform.position = _wings.transform.position + new Vector3(0, _srea.GetBounds().y, 0);
        Invoke(nameof(RemoveWings), .8f);
    }

    public void RemoveWings() => Destroy(_wings);

    public void OnEnable()
    {
        _ebf.LevelUp += CreateLevelUpWings;
        _hs.OnHealthChanged += WhenGetHit;
    }

    public void OnDisable()
    {
        _ebf.LevelUp -= CreateLevelUpWings;
        _hs.OnHealthChanged -= WhenGetHit;
    }

    // public void GoWeaponGo()
    // {
    //     List<EnemyCoxswain> lec = new List<EnemyCoxswain>(FindObjectsOfType<EnemyCoxswain>());
    //     List<Vector3> lv3 = new List<Vector3>();
    //
    //     foreach (EnemyCoxswain enemyCoxswain in lec)
    //     {
    //         lv3.Add(enemyCoxswain.transform.position);
    //     }
    //
    //     ArrowShaft.Setup(transform.position, lv3.ToArray());
    // }

    // public void SwipeWithSword()
    // {
    //     if (SwordSwipeGo)
    //     {
    //         _swordSwipe.SetActive(true);
    //
    //         if (_lastColliderPosition != transform.position)
    //         {
    //             _mainCol.offset += Vector2.right * .01f;
    //         }
    //
    //         Invoke(nameof(DisableEffect), EffectDuration);
    //     }
    // }

    // public void DisableEffect()
    // {
    //     _swordSwipe.SetActive(false);
    //
    //     if (_mainCol.offset != _defaultColliderOffset)
    //     {
    //         _mainCol.offset = _defaultColliderOffset;
    //     }
    // }

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

    // public void OnTriggerEnter2D(Collider2D collider2D)
    // {
        // if (collider2D.GetComponent<CoinOperator>())
        // {
        //     // Destroy(collider2D.gameObject);
        // }
    // }
}
