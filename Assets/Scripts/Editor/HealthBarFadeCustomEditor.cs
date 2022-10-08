using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HealthBarFade))]
public class HealthBarFadeCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HealthBarFade hbf = (HealthBarFade) target;
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
