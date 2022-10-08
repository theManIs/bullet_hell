using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarShrink : RectangleBar
{
    public float ShrinkSpeed = .2f;

    private float _damagedShrinkTimerMax = 1f;
    private float _damagedShrinkTimer;

    public override void Update()
    {
        _damagedShrinkTimer -= Time.deltaTime;

        if (_damagedShrinkTimer < 0)
        {
            if (_healthBar.fillAmount < _damageBar.fillAmount)
            {
                _damageBar.fillAmount -= ShrinkSpeed * Time.deltaTime;
            }
        }
    }

    public override void Awake()
    {
        _healthBar = transform.Find(HealthBarName).GetComponent<Image>();
        _damageBar = transform.Find(DamageBarName).GetComponent<Image>();

        _healthSystem = new HealthSystem(MaxPoints) { Health = StartPoints };
        // _healthSystem.OnHealthChanged += OnHealthChanged;
        _healthBar.fillAmount = _healthSystem.HealthPercent;
        _damageBar.fillAmount = _healthBar.fillAmount;
    }

    public override void OnHealthChanged(object sender, System.EventArgs args)
    {
        HealthSystemEventArguments hsea = sender as HealthSystemEventArguments;

        if (hsea != null && hsea.EventType == EventTypeSet.Damage)
        {
            _damagedShrinkTimer = _damagedShrinkTimerMax;
        }
        else
        {
            _damageBar.fillAmount = _healthBar.fillAmount;
        }

        _healthBar.fillAmount = _healthSystem.HealthPercent;
        
        FlashXTimes();
    }
}
