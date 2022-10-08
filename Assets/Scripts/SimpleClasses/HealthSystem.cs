using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;

    public float InvulnerabilityTime;

    public int Health;
    private int _healthMax;

    public int GetHealth => Health;
    public bool IsDead => Health <= 0;

    public float HealthPercent => (float)Health / _healthMax;

    private float _lastHit = 0;

    public HealthSystem(int health)
    {
        Health = Health != 0 ? Health : health;
        _healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        Health -= damageAmount;
        Health = Mathf.Clamp(Health, 0, _healthMax);
        // Debug.Log("Hit! Health: " + _health + " " + IsDead + " " + Time.frameCount);

        OnHealthChanged?.Invoke(this, new HealthSystemEventArguments { EventType = EventTypeSet.Damage, DamageAmount = damageAmount});
    }

    public void Heal(int healAmount)
    {
        Health += healAmount;
        Health = Mathf.Clamp(Health, 0, _healthMax);

        OnHealthChanged?.Invoke(this, new HealthSystemEventArguments{ EventType = EventTypeSet.Heal});
    }

    public void ApplyNormalizedDamage()
    {
        DamageWithInvulnerabilityTime(1);
    }

    public void DamageWithInvulnerabilityTime(int damageAmount)
    {

        if (Time.time - InvulnerabilityTime > _lastHit)
        {
            _lastHit = Time.time;

            Damage(damageAmount);
        }
    }

    // public void ResetSystem(int health)
    // {
    //     _health = health;
    //     _healthMax = health;
    // }
}
