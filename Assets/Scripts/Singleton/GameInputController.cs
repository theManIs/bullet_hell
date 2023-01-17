using System;
using UnityEngine;

public class GameInputController : MonoBehaviour
{
    public event Action OnEsc;
    public event Action On1;
    public event Action On2;
    public event Action On3;


    // public void OnEnable() => OnEsc += TestSubscription;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Debug.Log("Esc down");
            OnEsc?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            On1?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            On2?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            On3?.Invoke();
        }
    }

    // public void TestSubscription()
    // {
    //     Debug.Log("Test went well");
    // }

    public void PausePlay()
    {
        Time.timeScale = Time.timeScale != 0 ? 0 : 1;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    
    public void Play()
    {
        Time.timeScale = 1;
    }

    public bool IsPause()
    {
        return Time.timeScale == 0;
    }
}
