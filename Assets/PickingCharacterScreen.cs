using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PickingCharacterScreen : UIBase
{
    public event Action OnGo; 

    [Header("WeaponChoosing")]
    public List<Sprite> AvailableWeapons;
    public List<RectTransform> WeaponPositions;
    public Button ButtonLeft;
    public Button ButtonRight;
    public RectTransform FocusedWeaponFrame;
    public RectTransform FocusedWeapon;
    public int StartIndex = 0;
    public Button TakeButton;
    public Button DropButton;
    public int CurrentIndex = 0;
    [Header("CharacterPicking")] 
    public List<RectTransform> AvailableCharacters;
    public List<RectTransform> CharacterPositions;
    public List<Vector2> LocalPositions;
    public Button CharacterButtonLeft;
    public Button CharacterButtonRight;
    public RectTransform RightHand;
    public RectTransform RightHandWeapon;
    public RectTransform LeftHand;
    public RectTransform LeftHandWeapon;
    [Header("Miscellaneous")]
    public Button GoButton;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        gameObject.SetActive(true);

        ButtonLeft.onClick.AddListener(MoveTapeLeft);
        ButtonRight.onClick.AddListener(MoveTapeRight);

        int index = 0;
        foreach (RectTransform rt in WeaponPositions)
        {
            int localIndex = index;
            rt.GetComponent<Button>().onClick.AddListener(() => MoveFrame(rt, localIndex));
            index++;
        }
        MoveFrame(WeaponPositions[0], 0);

        LocalPositions = CharacterPositions.Select(t => t.anchoredPosition).ToList();

        CharacterButtonLeft.onClick.AddListener(MoveCarouselLeft);
        CharacterButtonRight.onClick.AddListener(MoveCarouselRight);

        GoButton.onClick.AddListener(() => OnGo?.Invoke());
        OnGo += () => gameObject.SetActive(false);

        TakeButton.onClick.AddListener(SpawnWeapon);
        DropButton.onClick.AddListener(EraseWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        RollWeaponCarousel();
        RollCharacterCarousel();
        MoveFrame(WeaponPositions[CurrentIndex], CurrentIndex);
    }

    public void SpawnWeapon()
    {
        if (!LeftHandWeapon)
        {
            LeftHandWeapon = Instantiate(FocusedWeapon, LeftHand);
            LeftHandWeapon.anchoredPosition = Vector2.zero;
            LeftHandWeapon.name = FocusedWeapon.GetComponent<Image>().sprite.name;
            TakeButton.gameObject.SetActive(false);
            DropButton.gameObject.SetActive(true);
        } 
        else if (!RightHandWeapon)
        {
            RightHandWeapon = Instantiate(FocusedWeapon, RightHand);
            RightHandWeapon.anchoredPosition = Vector2.zero;
            RightHandWeapon.name = FocusedWeapon.GetComponent<Image>().sprite.name;
            TakeButton.gameObject.SetActive(false);
            DropButton.gameObject.SetActive(true);
        }
    }

    public void SwitchTakeDrop()
    {
        TakeButton.gameObject.SetActive(!TakeButton.gameObject.activeSelf);
        DropButton.gameObject.SetActive(!DropButton.gameObject.activeSelf);
    }

    public void EraseWeapon()
    {

        if (LeftHandWeapon && LeftHandWeapon.name == FocusedWeapon.GetComponent<Image>().sprite.name)
        {
            Destroy(LeftHandWeapon.gameObject);
            LeftHandWeapon = null;
            SwitchTakeDrop();
        }

        if (RightHandWeapon && RightHandWeapon.name == FocusedWeapon.GetComponent<Image>().sprite.name)
        {
            Destroy(RightHandWeapon.gameObject);
            RightHandWeapon = null;
            SwitchTakeDrop();
        }
    }

    public void RollCharacterCarousel()
    {
        for (int i = 0; i < LocalPositions.Count; i++)
        {
            AvailableCharacters[i].anchoredPosition = LocalPositions[i];
        }
    }

    public void MoveCarouselRight()
    {
        LocalPositions.AddRange(LocalPositions.GetRange(0, 1));
        LocalPositions.RemoveAt(0);
    }

    public void MoveCarouselLeft()
    {
        LocalPositions.Insert(0, LocalPositions.GetRange(LocalPositions.Count - 1, 1)[0]);
        LocalPositions.RemoveAt(LocalPositions.Count - 1);
    }

    public void RollWeaponCarousel()
    {
        var list = AvailableWeapons.Skip(StartIndex).Take(WeaponPositions.Count);

        int index = 0;

        foreach (Sprite rt in list)
        {
            WeaponPositions[index].GetComponent<Image>().sprite = rt;
            // print(WeaponPositions[index].gameObject.GetComponent<Image>());
            index++;
        }
    }

    public void MoveTapeLeft() => StartIndex = Mathf.Clamp(StartIndex - 1, 0, AvailableWeapons.Count - 3);
    public void MoveTapeRight() => StartIndex = Mathf.Clamp(StartIndex + 1, 0, AvailableWeapons.Count - 3);

    public void MoveFrame(RectTransform rt, int index)
    {
        FocusedWeaponFrame.transform.position = rt.transform.position;
        FocusedWeapon = rt;
        CurrentIndex = index;

        SwitchBetweenButtons();
    }

    public void SwitchBetweenButtons()
    {
        string focusedName = FocusedWeapon.GetComponent<Image>().sprite.name;
        ;

        // if (LeftHandWeapon)
        // {
        //     print(LeftHandWeapon.name + " " + FocusedWeapon.GetComponent<Image>().sprite.name);
        // }
        //
        // if (RightHandWeapon)
        // {
        //     print(RightHandWeapon.name + " " + FocusedWeapon.GetComponent<Image>().sprite.name);
        // }

        if (LeftHandWeapon && LeftHandWeapon.name == focusedName || RightHandWeapon && RightHandWeapon.name == focusedName)
        {
            TakeButton.gameObject.SetActive(false);
            DropButton.gameObject.SetActive(true);
        }
        else if (LeftHandWeapon && RightHandWeapon)
        {
            TakeButton.gameObject.SetActive(false);
            DropButton.gameObject.SetActive(false);
        }
        else
        {
            TakeButton.gameObject.SetActive(true);
            DropButton.gameObject.SetActive(false);
        }

    }
}
