using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;
using UnityEngine;

[Serializable]
public class PerkProcessor
{
    public List<PerkScriptableObject> PerkScriptableObjects = new List<PerkScriptableObject>();

    public PerkProcessor Subscribe(OnPerkUpInterface ppp)
    {
        ppp.OnPerkUp += PerkUp;

        return this;
    }

    public void PerkUp(PerkType perkType)
    {
        bool levelUp = false;

        foreach (PerkScriptableObject pso in PerkScriptableObjects)
        {
            if (pso.PerkType == perkType)
            {
                pso.PerkLevel++;
                levelUp = true;
            }
        }

        if (!levelUp)
        {
            PerkScriptableObject pso = ScriptableObject.CreateInstance<PerkScriptableObject>();
            pso.PerkType = perkType;
            pso.PerkLevel = PerkLevel.One;
            // PerkScriptableObjects.Add(new PerkScriptableObject { PerkType = perkType, PerkLevel = PerkLevel.Zero });
            PerkScriptableObjects.Add(pso);
        }
    }
}
