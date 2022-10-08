using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HealthBars
{
    public HealthBarFade HealthBarFade;
    public HealthBarCut HealthBarCut;
    public HealthBarShrink HealthBarShrink;
}

public enum HealthBarsSet
{
    HealthBarFade,
    HealthBarCut,
    HealthBarShrink
}

public class DisplayControl : MonoBehaviour
{
    public HealthBars HealthBars;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
