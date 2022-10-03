using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererEffectAdder : MonoBehaviour
{
    private SpriteRenderer _affectedSpriteRenderer;
    private Color _initialColor;
    
    public void Awake()
    {
        _affectedSpriteRenderer = GetComponent<SpriteRenderer>();
        _initialColor = _affectedSpriteRenderer.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(BlinkColors), 0f, .3f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BlinkColors()
    {
        if (_affectedSpriteRenderer.color == _initialColor)
        {
            _affectedSpriteRenderer.color = Color.black;
        }
        else
        {
            
            _affectedSpriteRenderer.color = _initialColor;
        }
    }
}
