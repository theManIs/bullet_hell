using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldTreasury : MonoBehaviour
{
    public string CoinName = "Coin_2";

    private RectTransform _coin;

    public void Awake()
    {
        _coin = transform.Find(CoinName).GetComponent<RectTransform>();
    }

    public Vector2 GetCoinPosition()
    {
        return _coin.position;
    }

    public Vector2 GetCoinBottomCenter()
    {
        Vector2 rect = GetCoinPosition();
        rect.y -= _coin.sizeDelta.y / 2;

        return rect;
    }
}
