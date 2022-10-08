using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HealthBarCut))]
public class HealthBarCutCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HealthBarCut hbf = (HealthBarCut) target;
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
