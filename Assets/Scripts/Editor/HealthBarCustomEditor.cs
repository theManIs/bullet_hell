using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HealthBarFade))]
public class HealthBarCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HealthBarFade hbf = (HealthBarFade) target;

        if (GUILayout.Button("Damage"))
        {
            hbf.SetDamage(10);
        }
        
        if (GUILayout.Button("Heal"))
        {
            hbf.SetHeal(10);
        }
    }
}
