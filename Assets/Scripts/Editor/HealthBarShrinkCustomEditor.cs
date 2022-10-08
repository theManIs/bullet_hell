using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HealthBarShrink))]
public class HealthBarShrinkCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RectangleBar hbf = (RectangleBar) target;
        int pointValue = Mathf.CeilToInt(hbf.MaxPoints * .1f);

        if (GUILayout.Button("Damage"))
        {
            hbf.SetDamage(pointValue);
        }
        
        if (GUILayout.Button("Heal"))
        {
            hbf.SetHeal(pointValue);
        }
    }
}
