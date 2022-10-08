using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChangeExperience : MonoBehaviour
{
    public event Action<int> AddExperience;

    public ExperienceBarFade ExperienceBarFade;

    public void Awake()
    {
        ExperienceBarFade = FindObjectOfType<ExperienceBarFade>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void OnEnable() => AddExperience += ExperienceBarFade.GetHealthSystem().Heal;
    // public void OnDisable() => AddExperience += ExperienceBarFade.GetHealthSystem().Heal;

    public void InvokeEvent(int amount)
    {
        ExperienceBarFade.SetHeal(amount);
    }
}
