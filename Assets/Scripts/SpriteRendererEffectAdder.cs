using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererEffectAdder : MonoBehaviour
{
    private const int Pi = 180;
    private const int Pi2 = 360;

    [Range(0, 1)]
    public float RepelLevel = .1f;
    [Range(0, 180)]
    public float TiltMagnitude = 10;
    [Range(0, 100)]
    public float TiltSpeed = 1f;
    public bool RotateRight = true;

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

    public void SwingWhenMoving()
    {
        if (RotateRight)
        {
            transform.Rotate(0, 0, TiltSpeed * Time.deltaTime);

            if (transform.rotation.eulerAngles.z >= TiltMagnitude && transform.rotation.eulerAngles.z < Pi)
            {
                RotateRight = false;
            }
        }
        else
        {
            transform.Rotate(0, 0, -TiltSpeed * Time.deltaTime);

            //Debug.Log(transform.rotation.eulerAngles.z);

            if (transform.rotation.eulerAngles.z < Pi2 - TiltMagnitude && transform.rotation.eulerAngles.z > Pi)
            {
                RotateRight = true;
            }
        }
    }

}
