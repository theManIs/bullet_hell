using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarCut : HealthBarFade
{
    public string DamageBarTemplateName = "DamagedBarTemplate";

    private Transform _damagedBarTemplate;

    public override void Awake()
    {
        _healthBar = transform.Find(HealthBarName).GetComponent<Image>();
        _damagedBarTemplate = transform.Find(DamageBarTemplateName);

        _healthSystem = new HealthSystem(100);
        _healthSystem.OnHealthChanged += OnHealthChanged;
        _healthBar.fillAmount = _healthSystem.GetHealthPercent;
    }

    public override void OnHealthChanged(object sender, System.EventArgs args)
    {
//        Debug.Log("overriden method");

        float beforeDamageFillAmount = _healthBar.fillAmount;
        _healthBar.fillAmount = _healthSystem.GetHealthPercent;

        Transform damagedBar = Instantiate(_damagedBarTemplate, transform);
        damagedBar.gameObject.SetActive(true);
//        Debug.Log(_healthBar.GetComponent<RectTransform>().sizeDelta.x);
        damagedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(_healthBar.fillAmount * _healthBar.GetComponent<RectTransform>().sizeDelta.x + damagedBar.GetComponent<RectTransform>().anchoredPosition.x, damagedBar.GetComponent<RectTransform>().anchoredPosition.y);
        damagedBar.GetComponent<Image>().fillAmount = beforeDamageFillAmount - _healthBar.fillAmount;
        damagedBar.AddComponent<HealthBarCutFallDown>();
    }
}
