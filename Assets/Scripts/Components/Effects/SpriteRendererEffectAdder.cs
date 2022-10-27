using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererEffectAdder : MonoBehaviour
{
    private const int Pi = 180;
    private const int Pi2 = 360;
    
    // [Range(0, 1)]
    // public float RepelLevel = .1f;
    [Range(0, 180)]
    public float TiltMagnitude = 10;
    [Range(0, 100)]
    public float TiltSpeed = 1f;
    public bool RotateRight = true;

    private SpriteRenderer _affectedSpriteRenderer;
    private Color _initialColor;
    private Shader shaderGUItext;
    private Shader _defaultSpriteShader;
    private Animator _animator;

    public void Awake()
    {
        _affectedSpriteRenderer = GetComponent<SpriteRenderer>();
        _initialColor = _affectedSpriteRenderer.color;
        shaderGUItext = Shader.Find("GUI/Text Shader");
        _defaultSpriteShader = _affectedSpriteRenderer.material.shader;
        _animator = GetComponent<Animator>();
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

    public void SetInitialColor(Color c) => _initialColor = c;
    public Color GetInitialColor() => _initialColor;

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

    // public void RepelOneStepBack(Transform pointOfForce, float repelLevel)
    // {
    //     transform.position += (transform.position - pointOfForce.position).normalized * repelLevel;
    // }

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

    public void Move()
    {
        if (_animator)
        {
            _animator.SetBool("Moving", true);
        }
    }

    public void Stop()
    {
        if (_animator)
        {
            _animator.SetBool("Moving", false);
        }
    }

    public void FlipX(bool directionLeft)
    {
        _affectedSpriteRenderer.flipX = directionLeft;
    }
}
