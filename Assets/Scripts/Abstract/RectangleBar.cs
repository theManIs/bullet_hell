using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class RectangleBar : MonoBehaviour
{
    [Range(1, 100)]
    public int MaxPoints = 10;

    [Range(1, 100)]
    public int StartPoints = 1;

    public string HealthBarName = "HealthBar";
    public string DamageBarName = "DamageBar";
    public float DamageFadeTimerMax = 1f;
    public float DamageFadeTimer = 0;
    [Range(0, 10)]
    public float FlashTimer = 2;
    [Range(0, 1)]
    public float FlashThreshold = .3f;


    protected Image _healthBar;
    protected Image _damageBar;
    protected Color _healthInitCol;
    protected Color _damagedColor;
    protected HealthSystem _healthSystem;

    public virtual void Awake()
    {
        
    }

    public virtual void Start()
    {
        _healthInitCol = _healthBar.color;
    }

    public virtual void Update()
    {

    }

    public void OnEnable() => _healthSystem.OnHealthChanged += OnHealthChanged;
    public void OnDisable() => _healthSystem.OnHealthChanged -= OnHealthChanged;

    public virtual void OnHealthChanged(object sender, System.EventArgs args)
    {
        _healthBar.fillAmount = _healthSystem.HealthPercent;

        // for (float i = 0; i < FlashTimer; i += .6f)
        // {
        // int i = 0;
        //     Invoke(nameof(FlashHp), i);
        //     Invoke(nameof(FlashHp), .3f + i);
        //     Debug.Log(i + "  " + (.3f + i));
        // }
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

        // Debug.Log(_healthSystem.GetHealth);
    }

    public void FlashHp()
    {
        ;
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
            // Debug.Log(_healthBar.fillAmount > 0);
            if (_healthBar.fillAmount <= FlashThreshold)
            {
                // Debug.Log(_healthBar.fillAmount <= FlashThreshold);
                //                FlashTimer -= Time.deltaTime + FlashSpeed;
                //                Debug.Log(Time.frameCount % 3);
                //                if ((int)(_healthBar.fillAmount * 1000) % 3 == 0)
                //                if (FlashTimer <= 0)
                //                {
                //                FlashTimer = FlashTimerMax;
                _healthBar.color = _healthBar.color == _healthInitCol ? _healthInitCol * Color.gray : _healthInitCol;
                //                }
            }
        }
    }

    public void FlashXTimes()
    {
        if (_healthSystem.HealthPercent < FlashThreshold)
        {
            for (float i = 0; i < FlashTimer; i += .6f)
            {
                Invoke(nameof(FlashHp), i);
                Invoke(nameof(FlashHp), .3f + i);
                // Debug.Log(i + "  " + (.3f + i));
            }
        }
    }

    public HealthSystem GetHealthSystem()
    {
        return _healthSystem;
    }

    public void ReloadHealthSystem(int maxHealth, int health)
    {
        _healthSystem = new HealthSystem(maxHealth) { Health = health };
        _healthBar.fillAmount = _healthSystem.HealthPercent;
        _healthSystem.OnHealthChanged += OnHealthChanged;
    }
}
