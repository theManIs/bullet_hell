using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public const float OneMinute = 60.0f;

    public TextMeshProUGUI TextMesh;
    public int LevelDurationSeconds = 60;

    // Start is called before the first frame update
    void Start()
    {
        TextMesh = GetComponent<TextMeshProUGUI>();
        // print(TextMesh);

        // StartCoroutine(UpdateTimer());
        Invoke(nameof(StartCoroutineAfterStart), .0001f);
        // StartCoroutineAfterStart();
    }

    public void StartCoroutineAfterStart() => StartCoroutine(UpdateTimer());

    public IEnumerator UpdateTimer()
    {
        for (; LevelDurationSeconds >= 0;)
        {
            // print(LevelDurationSeconds + " " + Time.frameCount);
            ShowTimer();
            LevelDurationSeconds--;
            yield return new WaitForSeconds(1);
        }
    }

    public void ShowTimer()
    {
        int minutes = (int)MathF.Floor(LevelDurationSeconds / OneMinute);
        int seconds = LevelDurationSeconds % (int)OneMinute;
        TextMesh.text = $"{minutes:00}" + ":" + $"{seconds:00}";
    }
}
