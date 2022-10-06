using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class HealthBarFade : MonoBehaviour
{
    public string HealthBarName = "HealthBar";
    public string DamageBarName = "DamageBar";
    public float DamageFadeTimerMax = 1f;
    public float DamageFadeTimer = 0;
    public float FadeSpeed = 1f;
//    public float FlashTimer;
//    public float FlashTimerMax = .3f;
    public float FlashSpeed = .3f;


    protected Image _healthBar;
    protected Image _damageBar;
    protected Color _healthInitialColor;
    protected Color _damagedColor;
    protected HealthSystem _healthSystem;

    public virtual void Awake()
    {
        _healthBar = transform.Find(HealthBarName).GetComponent<Image>();
        _healthInitialColor = _healthBar.color;

        _damageBar = transform.Find(DamageBarName).GetComponent<Image>();
        _damagedColor = _damageBar.color;
        _damagedColor.a = 0f;
        _damageBar.color = _damagedColor;

        InvokeRepeating(nameof(FlashHp), 0, FlashSpeed);
        _healthSystem = new HealthSystem(100);
        _healthSystem.OnHealthChanged += OnHealthChanged;
        _healthBar.fillAmount = _healthSystem.GetHealthPercent;
    }

    // Start is called before the first frame update
    void Start()
    {
//        FlashTimer = FlashTimerMax;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (_damagedColor.a > 0f)
        {
            DamageFadeTimer -= Time.deltaTime;

            if (DamageFadeTimer < 0)
            {
                _damagedColor.a -= FadeSpeed * Time.deltaTime;
                _damageBar.color = _damagedColor;
            }
        }
    }

    public virtual void OnHealthChanged(object sender, System.EventArgs args)
    {
//        Debug.Log("virtual method");

        if (_damagedColor.a <= 0)
        {
            _damageBar.fillAmount = _healthBar.fillAmount;
        }
 
        _damagedColor.a = 1;
        _damageBar.color = _damagedColor;
        DamageFadeTimer = DamageFadeTimerMax;

        _healthBar.fillAmount = _healthSystem.GetHealthPercent;
    }

    public void SetDamage(int damageAmount)
    {
        _healthSystem.Damage(damageAmount);
//        _healthBar.fillAmount -= damageAmount * .01f;

        // Debug.Log(_healthSystem.GetHealth);
    }

    public void SetHeal(int healAmount)
    {
        _healthSystem.Heal(healAmount);
//        _healthBar.fillAmount += healAmount * .01f;

        Debug.Log(_healthSystem.GetHealth);
    }

    public void FlashHp()
    {
        if (_healthBar.fillAmount > 0)
        {
//            if (_healthBar.fillAmount < .3f)
//            {
//                _healthBar.fillAmount -= .001f;
//            }
//            else
//            {
//                _healthBar.fillAmount -= .01f;
//            }

            if (_healthBar.fillAmount <= .3f)
            {
//                FlashTimer -= Time.deltaTime + FlashSpeed;
//                Debug.Log(Time.frameCount % 3);
//                if ((int)(_healthBar.fillAmount * 1000) % 3 == 0)
//                if (FlashTimer <= 0)
//                {
//                FlashTimer = FlashTimerMax;
                _healthBar.color = _healthBar.color == Color.gray ? _healthInitialColor : Color.gray;
//                }
            }
        }
    }

    public HealthSystem GetHealthSystem()
    {
        return _healthSystem;
    }
}
