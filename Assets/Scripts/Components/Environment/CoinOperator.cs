using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CoinOperator : MonoBehaviour
{
    public event Action<int> PlusCoin;

    public bool StartMoving = false;
    public Vector3[] Destination = new Vector3[2];
    [Range(0,100)]
    public float CoinSpeed = 5;
    public string InteractionLayer = "Player";
    [Range(1, 100)][Header("Позволяет установить сколько моент прибавится к счёту голды")]
    public int CoinValue = 1;

    private DisplayControl _dc;
    private int _pathSection = 0;
    private OnTreasuryChange _otc;

    public void Awake()
    {
        _dc = FindObjectOfType<DisplayControl>();
        _otc = FindObjectOfType<OnTreasuryChange>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (StartMoving)
        {
            if (Destination.Length > _pathSection)
            {
                transform.position += (Destination[_pathSection] - transform.position).normalized * Time.deltaTime * CoinSpeed;
                // Debug.Log(Destination[_pathSection].sqrMagnitude + " " + transform.position.sqrMagnitude);
                // if ((Destination[_pathSection].sqrMagnitude - transform.position.sqrMagnitude) <= .1f)
                if (Vector3.Distance(Destination[_pathSection], transform.position) <= .1f)
                {
                    _pathSection += 1;
                }
            }
            else
            {
                PlusCoin?.Invoke(CoinValue);
                Destroy(gameObject);
            }
        }
    }

    public void OnEnable() => PlusCoin += _otc.IncreaseAccount;
    public void OnDisable() => PlusCoin += _otc.IncreaseAccount;

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        // if (collider2D.gameObject.GetComponent<CoinTaker>() != null)
        if (collider2D.gameObject.layer == LayerMask.NameToLayer(InteractionLayer))
        {
            StartMoving = true;

            // Vector2 rect = _dc.Treasury.GetComponent<GoldTreasury>().GetCoinPosition();
            // Vector3 viewport = Camera.main.ScreenToViewportPoint(rect);
            // Vector3 dest = Camera.main.ViewportToWorldPoint(viewport);
            // dest.z = 0;
            // Destination[0] = new Vector3(dest.x, transform.position.y);
            // Destination[1] = dest;
            Destination[0] = new Vector3(transform.position.x, transform.position.y + 1f);
            Destination[1] = new Vector3(transform.position.x, transform.position.y + 1f);
        }
    }

    public static void Setup(Vector3 position)
    {
        CoinOperator coin = GameAssets.CoinOperator;
        //Debug.Log(coin);
        Instantiate(coin.gameObject, position, Quaternion.identity);
    }
}
