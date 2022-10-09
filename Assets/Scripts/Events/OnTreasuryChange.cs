using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTreasuryChange : MonoBehaviour
{
    private DisplayControl _dc;

    public void Awake()
    {
        _dc = FindObjectOfType<DisplayControl>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void IncreaseAccount(int amount)
    {
        _dc.Treasury.text = (Convert.ToInt32(_dc.Treasury.text) + amount).ToString();
    }
}
