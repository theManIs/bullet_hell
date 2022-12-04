using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;
using UnityEngine;
using UnityEngine.UI;

public class PerkPanel : UIBase
{
    public static int PerkLimit;

    public event Action<PerkType> OnPerkLocked;
    public event Action<PerkType> OnPerkMax;

    private PickingPerkPanel _ppp;
    private readonly List<Transform> _pickedPerks = new List<Transform>();
    private readonly List<PerkScriptableObject> _psoObjects = new List<PerkScriptableObject>();

    public override void Start()
    {
        base.Start();
        gameObject.SetActive(true);

        _ppp = FindObjectOfType<PickingPerkPanel>();
        _ppp.OnPerkUp += AbilityUp;

        for (int i = 0; i < GetPerkHud().childCount; i++)
        {
            _pickedPerks.Add(GetPerkHud().GetChild(i));
        }

        PerkLimit = GetPerkHud().childCount;
    }

    private void AbilityUp(PerkType pt)
    {
        // Debug.Log(pt);
        if (!_psoObjects.Find(item => item.PerkType == pt))
        {
            PerkScriptableObject pso = PerkScriptableObject.GetPerkTemplate(pt);
            _psoObjects.Add(pso);
            OnPerkLocked?.Invoke(pt);
            // Debug.Log(pso.PerkLevel);
        }
        
        foreach (PerkScriptableObject psoObject in _psoObjects)
        {
            if (psoObject.PerkType == pt)
            {
                if (psoObject.PerkLevel != PerkLevel.Three)
                {
                    psoObject.PerkLevel++;
                    // Debug.Log(psoObject.PerkLevel);
                    if (psoObject.PerkLevel == PerkLevel.Three)
                    {
                        OnPerkMax?.Invoke(pt);
                    }
                }
            }
        }

        PerksLightUp();
    }

    public void PerksLightUp()
    {
        for (int i = 0; i < _psoObjects.Count; i++)
        {
            PerkScriptableObject psoOne = _psoObjects[i];
            // Debug.Log(_pickedPerks.Count);
            _pickedPerks[i].gameObject.SetActive(true);
            

            GetPerkImage(i).GetComponent<Image>().sprite = psoOne.PerkImage;

            for (int j = 0; j < GetPerkFrame(i).childCount; j++)
            {
                GetPerkFrame(i).GetChild(j).gameObject.SetActive(j < (int)psoOne.PerkLevel);
            }
        }
    }

    public Transform GetPerkHud() => transform.Find("PerkHUD");
    public Transform GetPerkImage(int index) => _pickedPerks[index].Find("PerkImage");
    public Transform GetPerkFrame(int index) => _pickedPerks[index].Find("PointsFrame");
}
