using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ExperienceBarFade))]
public class RectangleBarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ExperienceBarFade hbf = (ExperienceBarFade)target;
        int pointValue = Convert.ToInt32(hbf.MaxPoints * .1f);
        
        if (GUILayout.Button("Decrease"))
        {
            // Debug.Log(pointValue + " "  + hbf.GetHealthSystem().GetHealth);
            hbf.SetDamage(pointValue);
            // Debug.Log(pointValue + " " + hbf.GetHealthSystem().GetHealth);
        }

        if (GUILayout.Button("Increase"))
        {
            // Debug.Log(pointValue + " " + hbf.GetHealthSystem().GetHealth);
            hbf.SetHeal(pointValue);
            // Debug.Log(pointValue + " " + hbf.GetHealthSystem().GetHealth);
        }
    }
}