using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem
{
    public event EventHandler OnHealthChanged ;

    private int _health;
    private readonly int _healthMax;

    public int GetHealth => _health;
    public bool IsDead => _health <= 0;

    public float GetHealthPercent => (float)_health / _healthMax;

    public HealthSystem(int health)
    {
        _health = health;
        _healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;
        _health = Mathf.Clamp(_health, 0, _healthMax);

        OnHealthChanged?.Invoke(this, new HealthSystemEventArguments{ EventType = EventTypeSet.Damage });
    }

    public void Heal(int healAmount)
    {
        _health += healAmount;
        _health = Mathf.Clamp(_health, 0, _healthMax);

        OnHealthChanged?.Invoke(this, new HealthSystemEventArguments{ EventType = EventTypeSet.Heal});
    }

    public void ApplyNormalizedDamage()
    {
        Damage(1);
    }
}
