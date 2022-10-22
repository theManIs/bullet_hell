using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class DamageText : MonoBehaviour
{
    [Range(0, 100)]
    public float MoveSpeed = 1f;
    [Range(0, 100)]
    public float FadeSpeed = 1f;
    public Color TextColor;
    public Color CritColor;
    public int FontSize;
    public int CritSize;
    [Range(0, 1)]
    public float ScaleMultiplier = .3f;
    public float ScaleTimer = 1f;
    [Range(1, 10)]
    public float TextJumpMultiplier = 3;

    public int xSign;

    private TextMeshPro _textMeshPro;
    private Color _tmpColor;

    public void Start()
    {
        // Debug.Log(CritColor +  " " + TextColor);
        // _tmpColor = TextColor;
        _textMeshPro = GetComponent<TextMeshPro>();
        // _textMeshPro.color = IsCrit ? CritColor : TextColor;
        // _textMeshPro.color = IsCrit ? CritColor : TextColor;
    }
    
    void Update()
    {
        // if (_textMeshPro == null)
        // {
        //     _textMeshPro = GetComponent<TextMeshPro>();
        //     // _textMeshPro.color = CritColor;
        // }


        ScaleTimer -= Time.deltaTime;

        Vector3 v3 = transform.position;
        v3.x += Mathf.Abs(ScaleTimer - .5f) * TextJumpMultiplier * Time.deltaTime * MoveSpeed * xSign;
        v3.y += (ScaleTimer - .5f) * TextJumpMultiplier * Time.deltaTime * MoveSpeed;
        transform.position = v3;

        transform.localScale += Vector3.one * Time.deltaTime * Mathf.Sign(ScaleTimer - .5f) * ScaleMultiplier;

        if (ScaleTimer < 0)
        {
            _tmpColor = _textMeshPro.color;
            _tmpColor.a -= Time.deltaTime * FadeSpeed;
            _textMeshPro.color = _tmpColor;
            // transform.position += Vector3.up * Time.deltaTime * MoveSpeed;
        }

        if (_textMeshPro.color.a < 0)
        {
            Destroy(gameObject);
        }
    }

    public static void Setup(Vector3 position, int amount, bool crit, int xSign)
    {
        DamageText resource = GameAssets.DamageText;
        TextMeshPro _textMeshPro = Instantiate(resource.GetComponent<TextMeshPro>(), position, Quaternion.identity);

        _textMeshPro.SetText(amount.ToString());
        DamageText instance = _textMeshPro.GetComponent<DamageText>();
        instance.xSign = xSign;

        if (crit)
        {
            _textMeshPro.color = instance.CritColor;
            _textMeshPro.fontSize = instance.CritSize;
            _textMeshPro.rectTransform.sizeDelta *= instance.CritSize / (float)instance.FontSize;
        }
        else
        {
            _textMeshPro.color = instance.TextColor;
        }
    }
}
