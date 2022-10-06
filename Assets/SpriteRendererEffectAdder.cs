using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererEffectAdder : MonoBehaviour
{
    public float RepelLevel = .1f;

    private SpriteRenderer _affectedSpriteRenderer;
    private Color _initialColor;
    private Shader shaderGUItext;
    private Shader _defaultSpriteShader;

    public void Awake()
    {
        _affectedSpriteRenderer = GetComponent<SpriteRenderer>();
        _initialColor = _affectedSpriteRenderer.color;
        shaderGUItext = Shader.Find("GUI/Text Shader");
        _defaultSpriteShader = _affectedSpriteRenderer.material.shader;
    }

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating(nameof(BlinkColors), 0f, .3f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BlinkColors()
    {
        if (_affectedSpriteRenderer.material.shader == _defaultSpriteShader)
        {
            //Debug.Log("Set white");
            _affectedSpriteRenderer.material.shader = shaderGUItext;
            _affectedSpriteRenderer.color = Color.white;
        }
        else
        {
            _affectedSpriteRenderer.material.shader = _defaultSpriteShader;
            _affectedSpriteRenderer.color = _initialColor;
            //Debug.Log("Set normal");
        }
    }

    public void BlinkOnce()
    {
        BlinkColors();

        Invoke(nameof(BlinkColors), .3f);
    }

    public void RepelOneStepBack(Transform pointOfForce)
    {
        transform.position += (transform.position - pointOfForce.position).normalized * RepelLevel;
    }
}
