using System;
using System.Collections.Generic;
using Assets.Scripts.Types;
using UnityEngine;
using UnityEngine.UI;


public class PickingPerkPanel : UIBase, OnPerkUpInterface
{
    public event Action<PerkType> OnPerkUp;
    public List<Button> PerkCards;
    private GameInputController _gic;

    // public override void Start()
    // {
    //     base.Start();
    //
    //     gameObject.SetActive(true);
    // }

    public void Awake()
    {
        PerkCards = new List<Button>(GetComponentsInChildren<Button>());
        _gic = FindObjectOfType<GameInputController>();

        // PerkCards[0].onClick.AddListener(() => PerkUp(PerkType.WeaponCooldown));
        // PerkCards[1].onClick.AddListener(() => PerkUp(PerkType.WeaponDamage));
        // PerkCards[2].onClick.AddListener(() => PerkUp(PerkType.WeaponRange));
    }

    public void PerkUp(PerkType perkType)
    {
        OnPerkUp?.Invoke(perkType);
    }

    public void ShowUp()
    {

        if (!_gic.IsPause())
        {
            gameObject.SetActive(true);
            Time.timeScale = 0;

            PerkCards[0].onClick.AddListener(() => PerkUp(PerkType.WeaponCooldown));
            PerkCards[1].onClick.AddListener(() => PerkUp(PerkType.WeaponDamage));
            PerkCards[2].onClick.AddListener(() => PerkUp(PerkType.WeaponRange));
        }
        else
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
