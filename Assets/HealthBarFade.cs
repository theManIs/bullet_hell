using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class HealthBarFade : MonoBehaviour
{
    public string HealthBarName = "HealthBar";
    
    
    private Image _healthBar;
    private Color _healthInitialColor;
    private HealthSystem _healthSystem;


    public void Awake()
    {
        _healthBar = transform.Find(HealthBarName).GetComponent<Image>();
        _healthInitialColor = _healthBar.color;
        InvokeRepeating(nameof(FlashHp), 0, .03f);
        _healthSystem = new HealthSystem(100);

        _healthSystem.OnHealthChanged += OnHealthChanged;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnHealthChanged(object sender, System.EventArgs args)
    {
        _healthBar.fillAmount = _healthSystem.GetHealthPercent;
    }

    public void SetDamage(int damageAmount)
    {
        _healthSystem.Damage(damageAmount);
//        _healthBar.fillAmount -= damageAmount * .01f;

        Debug.Log(_healthSystem.GetHealth);
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
//                Debug.Log(Time.frameCount % 3);
//                if ((int)(_healthBar.fillAmount * 1000) % 3 == 0)
                if ((int)(Random.value * 1000) % 2 == 0)
                {
                    _healthBar.color = Color.white;
                }
                else
                {
                    _healthBar.color = _healthInitialColor;
                }
            }
        }

        
    }
}
