using UnityEngine;
using UnityEngine.UI;

public class HealthBarCutFallDown : MonoBehaviour
{
    private RectTransform _rectTransform;
    private float _fallDownTimer;
    private float _fadeTimer = 1f;
    private Image _image;
    private Color _color;

    private void Awake()
    {
        _rectTransform = transform.GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _fallDownTimer = 1f;
        _color = _image.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _fallDownTimer -= Time.deltaTime;

        if (_fallDownTimer < 0)
        {
            float fallSpeed = 20f;

            _rectTransform.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime; 
        }

        _fadeTimer -= Time.deltaTime;

        if (_fadeTimer < 0)
        {
            float alphaFadeSpeed = 1f;
            _color.a -= alphaFadeSpeed * Time.deltaTime;
            _image.color = _color;

            if (_color.a < 0)
            {
                Destroy(gameObject);
            }
        }

    }
}
