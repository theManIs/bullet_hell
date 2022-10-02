using UnityEngine;
using UnityEngine.UI;

public class KnightCoxswain : MonoBehaviour
{
    public GameObject SwordSwipeGo;
    public float SwipeFrequency = 1f;
    public float EffectDuration = 1f;

    public float MoveSpeed = 2;

    private GameObject _swordSwipe;
    private SpriteRenderer _knightSp;
    private SpriteRenderer _swipeSp;

    // Start is called before the first frame update
    void Start()
    {
        _knightSp = GetComponent<SpriteRenderer>();

        if (SwordSwipeGo)
        {
            _swordSwipe = Instantiate(SwordSwipeGo, transform);
            _swordSwipe.SetActive(false);
            _swipeSp = _swordSwipe.GetComponent<SpriteRenderer>();

            InvokeRepeating(nameof(SwipeWithSword), SwipeFrequency, SwipeFrequency);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

//        Debug.Log(xAxis + " " + yAxis);

        if (!xAxis.Equals(0) || !yAxis.Equals(0))
        {
            transform.position += Vector3.right * Time.deltaTime * MoveSpeed * xAxis + Vector3.up * Time.deltaTime * MoveSpeed * yAxis;

            _knightSp.flipX = !(xAxis > 0);
            _swipeSp.flipX = !(xAxis > 0);
        }
    }

    public void SwipeWithSword()
    {
        if (SwordSwipeGo)
        {
            _swordSwipe.SetActive(true);

            Invoke(nameof(DisableEffect), EffectDuration);
        }
    }

    public void DisableEffect()
    {
        _swordSwipe.SetActive(false);
    }
}
