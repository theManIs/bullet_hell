using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;

    public float InvulnerabilityTime;

    private int _health;
    private int _healthMax;

    public int GetHealth => _health;
    public bool IsDead => _health <= 0;

    public float GetHealthPercent => (float)_health / _healthMax;

    private float _lastHit = 0;

    public HealthSystem(int health)
    {
        _health = health;
        _healthMax = health;
    }

    public int Damage(int damageAmount)
    {
        _health -= damageAmount;
        _health = Mathf.Clamp(_health, 0, _healthMax);
        // Debug.Log("Hit! Health: " + _health + " " + IsDead + " " + Time.frameCount);

        OnHealthChanged?.Invoke(this, new HealthSystemEventArguments { EventType = EventTypeSet.Damage, DamageAmount = damageAmount});

        return damageAmount;
    }

    public void Heal(int healAmount)
    {
        _health += healAmount;
        _health = Mathf.Clamp(_health, 0, _healthMax);

        OnHealthChanged?.Invoke(this, new HealthSystemEventArguments{ EventType = EventTypeSet.Heal});
    }

    public int ApplyNormalizedDamage()
    {
        return DamageWithInvulnerabilityTime(1);
    }

    public int DamageWithInvulnerabilityTime(int damageAmount)
    {

        if (Time.time - InvulnerabilityTime > _lastHit)
        {
            _lastHit = Time.time;

            return Damage(damageAmount);
        }

        return 0;
    }

    // public void ResetSystem(int health)
    // {
    //     _health = health;
    //     _healthMax = health;
    // }
}
