using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGamePanel : UIBase
{
    public string BonusPanel = "BonusPanel";

    public int VerticalShift = -75;
    public TextMeshProUGUI BonusTextColumnOne;
    public TextMeshProUGUI BonusTextColumnTwo;
    public FinalScreenScriptableObject Fsso;
    public LevelTimer LevelTimer;

    public override void Start()
    {
        base.Start();
        // gameObject.SetActive(true);

        BonusTextColumnOne = transform.Find(BonusPanel).GetChild(0).GetComponent<TextMeshProUGUI>();
        BonusTextColumnTwo = transform.Find(BonusPanel).GetChild(1).GetComponent<TextMeshProUGUI>();

        BonusTextColumnOne.gameObject.SetActive(false);
        BonusTextColumnTwo.gameObject.SetActive(false);

        Fsso = GameAssets.FinalScreenScriptableObject;

        ShowBonuses();
        LevelTimer = FindObjectOfType<LevelTimer>();
        LevelTimer.OnTimeUp += SetBonusesActive;
    }

    // public void OnEnable() => LevelTimer.OnTimeUp += SetBonusesActive;

    public void SetBonusesActive() => gameObject.SetActive(true);

    public void ShowBonuses()
    {
        int number = 0;

        foreach (Bonus bonus in Fsso.Bonuses)
        {
            TextMeshProUGUI tmpgui;
            int level = Mathf.FloorToInt(number / 2f);
            
            if (number % 2 != 0)
            {
                tmpgui = Instantiate(BonusTextColumnOne, transform.Find(BonusPanel));
                Vector2 rectPos = BonusTextColumnOne.rectTransform.anchoredPosition + new Vector2(0, VerticalShift * level);
                tmpgui.rectTransform.anchoredPosition = rectPos;
                tmpgui.text = bonus.BonusTitle + " x" + bonus.BonusEffect;
            }
            else
            {
                tmpgui = Instantiate(BonusTextColumnTwo, transform.Find(BonusPanel));
                Vector2 rectPos = BonusTextColumnTwo.rectTransform.anchoredPosition + new Vector2(0, VerticalShift * level);
                tmpgui.rectTransform.anchoredPosition = rectPos;
                tmpgui.text = bonus.BonusTitle + " x" + bonus.BonusEffect;
            }

            tmpgui.gameObject.SetActive(true);
            number++;
        }
    }
}
