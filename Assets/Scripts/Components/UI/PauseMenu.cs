using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : UIBase
{
    public override void Start()
    {
        base.Start();

        FindObjectOfType<GameInputController>().OnEsc += PauseGame;
    }

    // public void OnEnable() => FindObjectOfType<GameInputController>().OnEsc += PauseGame;
    // public void OnDisable() => FindObjectOfType<GameInputController>().OnEsc -= PauseGame;

    public void PauseGame()
    {
        // if (Time.timeScale == 0)
        // {
        //     gameObject.SetActive(false);
        //     Time.timeScale = 1;
        // }
        // else
        // {
        //     gameObject.SetActive(true);
        //     Time.timeScale = 0;
        // }

        // Debug.Log("Pause On " + Time.timeScale);
        gameObject.SetActive(Time.timeScale != 0);
        Time.timeScale = Convert.ToInt32(Time.timeScale == 0);
        // Debug.Log(Time.timeScale);
    }

    ~PauseMenu()
    {
        FindObjectOfType<GameInputController>().OnEsc -= PauseGame;
    }
}
