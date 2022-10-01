using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem
{
    public event EventHandler OnHealthChanged ;

    private int _health;
    private int _healtMax;

    public int GetHealth => _health;
    public float GetHealthPercent => (float)_health / _healtMax;

    public HealthSystem(int health)
    {
        _health = health;
        _healtMax = health;
    }

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;
        _health = Mathf.Clamp(_health, 0, _healtMax);

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(int healAmount)
    {
        _health += healAmount;
        _health = Mathf.Clamp(_health, 0, _healtMax);

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }


}
