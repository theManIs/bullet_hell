using System;
using UnityEngine;

public class GameInputController : MonoBehaviour
{
    public event Action OnEsc;

    // public void OnEnable() => OnEsc += TestSubscription;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Esc down");
            OnEsc?.Invoke();
        }
    }

    // public void TestSubscription()
    // {
    //     Debug.Log("Test went well");
    // }

    public void Pause()
    {
        Time.timeScale = Time.timeScale != 0 ? 0 : 1;
    }

    public bool IsPause()
    {
        return Time.timeScale == 0;
    }
}
