using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CoinOperator : MonoBehaviour
{
    public bool StartMoving = false;
    public Vector3[] Destination = new Vector3[2];
    public float CoinSpeed = 5;

    private DisplayControl _dc;
    private int _pathSection = 0;

    public void Awake()
    {
        _dc = FindObjectOfType<DisplayControl>();
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
        }
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.GetComponent<CoinTaker>() != null)
        {
            StartMoving = true;

            // Destination = new Vector3(-9, transform.position.y);

            Vector2 rect = _dc.Treasury.rectTransform.position;
            rect.y -= _dc.Treasury.rectTransform.sizeDelta.y / 2;
            // Debug.Log(_dc.Treasury.rectTransform.sizeDelta.y + " " + rect);
            Vector3 viewport = Camera.main.ScreenToViewportPoint(rect);
            Vector3 dest = Camera.main.ViewportToWorldPoint(viewport);
            dest.z = 0;
            Destination[0] = new Vector3(dest.x, transform.position.y);
            Destination[1] = dest;

            // Debug.Log(Destination + " " + collider2D.gameObject.name);
            // Debug.Log(rect + " "  + viewport + " " + dest);
        }
    }
}
