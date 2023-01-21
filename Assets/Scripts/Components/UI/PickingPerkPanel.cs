using System;
using System.Collections.Generic;
using Assets.Scripts.Types;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PickingPerkPanel : UIBase, OnPerkUpInterface
{
    public static int PerkLimit;

    public event Action<PerkType> OnPerkUp;
    public List<Button> PerkButtons;
    public List<Image> PerkImages;
    public string PerkImageName = "PerkImage";
    public string PerkTextName = "PerkText";
    public string PerkIconName = "PerkIcon";
    public bool PanelOn = false;
    public int PreStartLimit = 5;
    public List<PerkType> LockedPerks;
    public List<PerkType> PerpetualBlockedPerks = new List<PerkType>();
    public List<int> PerpetualBlockedIds = new List<int>();

    private readonly List<Action> _buttonActions = new List<Action>(3);

    private GameInputController _gic;
    private ExperienceBarFade _ebe;
    private PerkPanel _pp;

    public override void Start()
    {
        base.Start();

        if (PanelOn)
        {
            ShowUp();
        }
        // print(Time.time);
    }

    public void Awake()
    {
        PerkButtons = new List<Button>(GetComponentsInChildren<Button>());
        _gic = FindObjectOfType<GameInputController>();

        PerkImages = new List<Image>(GetComponentsInChildren<Image>()).FindAll(a => a.gameObject.name == PerkImageName);
        PerkLimit = PerkImages.Count;
        // PerkCards[0].onClick.AddListener(() => PerkUp(PerkType.WeaponCooldown));
        // PerkCards[1].onClick.AddListener(() => PerkUp(PerkType.WeaponDamage));
        // PerkCards[2].onClick.AddListener(() => PerkUp(PerkType.WeaponRange));

        _ebe = FindObjectOfType<ExperienceBarFade>();
        _ebe.LevelUp += ShowUp;
        _pp = FindObjectOfType<PerkPanel>();
        _pp.OnPerkLocked += LockPerk;
        _pp.OnPerkMax += BlockPerk;
    }

    public void LockPerk(PerkType pt) => LockedPerks.Add(pt);
    public void BlockPerk(PerkType pt)
    {
        PerpetualBlockedPerks.Add(pt);

        for (int i = 0; i < LockedPerks.Count; i++)
        {
            if (LockedPerks[i] == pt)
            {
                PerpetualBlockedIds.Add(i);
            }
        }
    }

    public void PerkUp(PerkType perkType)
    {
        OnPerkUp?.Invoke(perkType);

        gameObject.SetActive(false);
        _gic.Play();

        //TODO Придумать как заменить это циклом
        if (_buttonActions.Count > 0)
        {
            _gic.On1 -= _buttonActions[0];
        }

        if (_buttonActions.Count > 1)
        {
            _gic.On2 -= _buttonActions[1];
        }
        
        if (_buttonActions.Count > 2)
        {
            _gic.On3 -= _buttonActions[2];
        }
        //TODO Придумать как обращаться к индексам, а не стирать весь объект
        _buttonActions.Clear();

        if (PanelOn && PreStartLimit > 0)
        {
            ShowUp();
            PreStartLimit--;
        }
    }

    public int RollUniqueInt(List<int> blockedInts)
    {
        int randomInt = Random.Range(0, LockedPerks.Count);

        return blockedInts.Count == LockedPerks.Count ? -1 : !blockedInts.Contains(randomInt) ? randomInt : RollUniqueInt(blockedInts);
    }

    public void ShowUp()
    {
        if (LockedPerks.Count == PerkPanel.PerkLimit && PerpetualBlockedPerks.Count >= LockedPerks.Count)
        {
            return;
        }

        if (!_gic.IsPause())
        {
            gameObject.SetActive(true);
            _gic.Pause();
            List<PerkType> blockedPerks = new List<PerkType>(PerpetualBlockedPerks);
            List<int> blockedPerkIds = new List<int>(PerpetualBlockedIds);

            // Debug.Log(lockedPerks.Count);
            for (int i = 0; i < PerkImages.Count; i++)
            {
                if (LockedPerks.Count == PerkPanel.PerkLimit && blockedPerkIds.Count >= LockedPerks.Count)
                {
                    PerkButtons[i].transform.parent.gameObject.SetActive(false);
                    break;
                }

                // Debug.Log(PerkScriptableObject.RollPerk());
                PerkType pt;
                // if (LockedPerks.Count == PerkPanel.PerkLimit)
                // {
                //     int randomInt = RollUniqueInt(blockedPerkIds);
                //     if (randomInt == -1)
                //     {
                //         pt = default;
                //     }
                //     else
                //     {
                //         pt = LockedPerks[randomInt];
                //         blockedPerkIds.Add(randomInt);
                //     }
                // }
                // else 
                if (LockedPerks.Count - PerpetualBlockedPerks.Count > PerkPanel.PerkLimit - PerkLimit 
                         && LockedPerks.Count - (PerkPanel.PerkLimit - PerkLimit) > i
                         && !blockedPerks.Contains(LockedPerks[i])
                         || LockedPerks.Count == PerkPanel.PerkLimit)
                {
                    // Debug.Log(lockedPerks.Count - (PerkPanel.PerkLimit - PerkLimit));
                    // pt = LockedPerks[i];
                    int randomInt = RollUniqueInt(blockedPerkIds);
                    if (randomInt == -1)
                    {
                        pt = default;
                    }
                    else
                    {
                        pt = LockedPerks[randomInt];
                        blockedPerkIds.Add(randomInt);
                    }
                }
                else
                {
                    pt = PerkScriptableObject.RollPerkUnique(blockedPerks);
                }

                PerkScriptableObject perk1 = PerkScriptableObject.GetPerkTemplate(pt);
                blockedPerks.Add(pt);
                PerkButtons[i].onClick.RemoveAllListeners();
                PerkButtons[i].onClick.AddListener(() => PerkUp(perk1.PerkType));
                // Debug.Log(PerkCards[i].transform.parent.Find(PerkIconName));
                // Debug.Log(PerkCards[i].transform.parent.Find(PerkIconName).Find("PerkImage").GetComponent<Image>());
                PerkButtons[i].transform.parent.Find(PerkIconName).Find(PerkImageName).GetComponent<Image>().sprite = perk1.PerkImage;
                // Debug.Log(PerkCards[i].transform.parent);
                // Debug.Log(PerkCards[i].transform.parent.Find("PerkText"));
                PerkButtons[i].transform.parent.Find(PerkTextName).GetComponent<TextMeshProUGUI>().text = perk1.PerkDescription;
                
                Action perkCallback = () => PerkUp(perk1.PerkType);
                _buttonActions.Add(perkCallback);
                
                switch (i)
                {
                    case 0: _gic.On1 += _buttonActions[0]; break;
                    case 1: _gic.On2 += _buttonActions[1]; break;
                    case 2: _gic.On3 += _buttonActions[2]; break; 
                }
            }
        }
        else
        {
            gameObject.SetActive(false);
            _gic.Play();
        }
    }
}
