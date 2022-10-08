using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class HealthBarFade : RectangleBar
{
    public float FadeSpeed = 1f;
    public float FlashSpeed = .3f;

    public override void Awake()
    {
        _healthBar = transform.Find(HealthBarName).GetComponent<Image>();
        _healthInitCol = _healthBar.color;

        _damageBar = transform.Find(DamageBarName).GetComponent<Image>();
        _damagedColor = _damageBar.color;
        _damagedColor.a = 0f;
        _damageBar.color = _damagedColor;

        // InvokeRepeating(nameof(FlashHp), 0, FlashSpeed);
        _healthSystem = new HealthSystem(MaxPoints) { Health = StartPoints };
        // _healthSystem.OnHealthChanged += OnHealthChanged;
        _healthBar.fillAmount = _healthSystem.HealthPercent;
    }

    public override void Update()
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

    public override void OnHealthChanged(object sender, System.EventArgs args)
    {
        if (_damagedColor.a <= 0)
        {
            _damageBar.fillAmount = _healthBar.fillAmount;
        }
 
        _damagedColor.a = 1;
        _damageBar.color = _damagedColor;
        DamageFadeTimer = DamageFadeTimerMax;

        _healthBar.fillAmount = _healthSystem.HealthPercent;

        FlashXTimes();
    }
}
