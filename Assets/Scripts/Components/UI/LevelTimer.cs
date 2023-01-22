using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public event Action OnTimeUp;

    public const float OneMinute = 60.0f;

    public TextMeshProUGUI TextMesh;
    public int LevelDurationSeconds = 60;

    public void Awake()
    {
        FindObjectOfType<PickingCharacterScreen>().OnGo += () => Invoke(nameof(StartCoroutineAfterStart), .0001f);
    }

    // Start is called before the first frame update
    public void Start()
    {
        TextMesh = GetComponent<TextMeshProUGUI>();
        // print(TextMesh);

        // StartCoroutine(UpdateTimer());
        // Invoke(nameof(StartCoroutineAfterStart), .0001f);
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

        if (LevelDurationSeconds <= 0)
        {
            OnTimeUp?.Invoke();
        }
    }

    public void ShowTimer()
    {
        int minutes = (int)MathF.Floor(LevelDurationSeconds / OneMinute);
        int seconds = LevelDurationSeconds % (int)OneMinute;
        TextMesh.text = $"{minutes:00}" + ":" + $"{seconds:00}";
    }

    public void TimeUpInvoke()
    {
        OnTimeUp?.Invoke();
    }
}
