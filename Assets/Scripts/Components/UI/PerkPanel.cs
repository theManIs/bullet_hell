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

    public string PerkHud = "PerkHUD";
    public string PerkImage = "PerkImage";
    public string PointsFrame = "PointsFrame";

    private PickingPerkPanel _ppp;
    private readonly List<Transform> _pickedPerks = new List<Transform>();
    private readonly List<PerkScriptableObject> _psoObjects = new List<PerkScriptableObject>();
    private readonly List<List<Transform>> _llt = new List<List<Transform>>();

    public override void Start()
    {
        base.Start();
        gameObject.SetActive(true);

        

        FillTheLists();

        PerkLimit = GetPerkHud().childCount;
    }

    public void OnEnable()
    {
        _ppp = FindObjectOfType<PickingPerkPanel>();
        // Debug.Log(FindObjectOfType<PickingPerkPanel>());

        if (_ppp)
        {
            _ppp.OnPerkUp += AbilityUp;
        }
    }

    private void FillTheLists()
    {
        for (int i = 0; i < GetPerkHud().childCount; i++)
        {
            _pickedPerks.Add(GetPerkHud().GetChild(i));
            GetPerkHud().GetChild(i).gameObject.SetActive(false);
            List<Transform> lt = new List<Transform>();

            for (int j = 0; j < GetPerkFrame(i).childCount; j++)
            {
                lt.Add(GetPerkFrame(i).GetChild(j));
            }

            _llt.Add(lt);
        }
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

            _llt[i].ForEach(item => item.gameObject.SetActive(item.GetSiblingIndex() < (int)psoOne.PerkLevel));

            // for (int j = 0; j < GetPerkFrame(i).childCount; j++)
            // {
            //     GetPerkFrame(i).GetChild(j).gameObject.SetActive(j < (int)psoOne.PerkLevel);
            // }
        }
    }

    public Transform GetPerkHud() => transform.Find(PerkHud);
    public Transform GetPerkImage(int index) => _pickedPerks[index].Find(PerkImage);
    public Transform GetPerkFrame(int index) => _pickedPerks[index].Find(PointsFrame);
}
