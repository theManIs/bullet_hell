using UnityEngine;
using UnityEngine.UI;

public class ExperienceBarFade : RectangleBar
{
    private readonly LevelingLedger _ll = new LevelingLedger();

    public override void Awake()
    {
        _healthBar = transform.Find(HealthBarName).GetComponent<Image>();

        _healthSystem = new HealthSystem(_ll.CurrentExperience) { Health = StartPoints };
        // _healthSystem.OnHealthChanged += OnHealthChanged;
        _healthBar.fillAmount = _healthSystem.HealthPercent;

        // Debug.Log(_healthSystem.Health + " " + _healthSystem.GetHealth  + " " + _healthBar.fillAmount);
    }

    public override void OnHealthChanged(object sender, System.EventArgs args)
    {
        _healthBar.fillAmount = _healthSystem.HealthPercent;
        // Debug.Log(_healthBar.fillAmount);
        if (_healthBar.fillAmount >= 1)
        {
            _ll.LevelUp();
            ReloadHealthSystem(_ll.CurrentExperience, 0);
            // _healthSystem = new HealthSystem(_ll.CurrentExperience) { Health = 0 };
            // _healthBar.fillAmount = _healthSystem.HealthPercent;
            // _healthSystem.OnHealthChanged += OnHealthChanged;
            // Debug.Log(_healthSystem.GetHealth);
        }
    }
}
