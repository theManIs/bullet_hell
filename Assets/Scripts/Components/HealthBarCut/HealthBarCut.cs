using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarCut : RectangleBar
{
    // public string DamageBarTemplateName = "DamagedBarTemplate";
    public int RealImagePixelErrorX = 0;

    private Transform _damagedBarTemplate;

    public override void Awake()
    {
        _healthBar = transform.Find(HealthBarName).GetComponent<Image>();
        _damagedBarTemplate = transform.Find(DamageBarName);

        _healthSystem = new HealthSystem(MaxPoints) { Health = StartPoints };
        // _healthSystem.OnHealthChanged += OnHealthChanged;
        _healthBar.fillAmount = _healthSystem.HealthPercent;
    }

    public override void OnHealthChanged(object sender, System.EventArgs args)
    {
//        Debug.Log("overriden method");

        float beforeDamageFillAmount = _healthBar.fillAmount;
        _healthBar.fillAmount = _healthSystem.HealthPercent;

        Transform damagedBar = Instantiate(_damagedBarTemplate, transform);
        damagedBar.gameObject.SetActive(true);
        damagedBar.SetSiblingIndex(transform.GetSiblingIndex() + 1);
//        Debug.Log(_healthBar.GetComponent<RectTransform>().sizeDelta.x);
        float xPos = _healthBar.fillAmount * _healthBar.GetComponent<RectTransform>().sizeDelta.x - RealImagePixelErrorX + damagedBar.GetComponent<RectTransform>().anchoredPosition.x;
        damagedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, damagedBar.GetComponent<RectTransform>().anchoredPosition.y);
        damagedBar.GetComponent<Image>().fillAmount = beforeDamageFillAmount - _healthBar.fillAmount;
        damagedBar.AddComponent<HealthBarCutFallDown>();

        FlashXTimes();
    }
}
