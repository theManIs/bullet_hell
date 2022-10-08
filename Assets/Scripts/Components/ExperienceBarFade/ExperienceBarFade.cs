using UnityEngine;
using UnityEngine.UI;

public class ExperienceBarFade : RectangleBar
{
    public override void Awake()
    {
        _healthBar = transform.Find(HealthBarName).GetComponent<Image>();

        _healthSystem = new HealthSystem(MaxPoints) { Health = StartPoints };
        // _healthSystem.OnHealthChanged += OnHealthChanged;
        _healthBar.fillAmount = _healthSystem.HealthPercent;

        // Debug.Log(_healthSystem.Health + " " + _healthSystem.GetHealth  + " " + _healthBar.fillAmount);
    }

    public override void OnHealthChanged(object sender, System.EventArgs args)
    {
        _healthBar.fillAmount = _healthSystem.HealthPercent;
    }
}
