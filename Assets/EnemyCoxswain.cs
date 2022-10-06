using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoxswain : MonoBehaviour
{
    [Header("Rewrite this code")]
    public GameObject target;

    [Space]
    public bool RotateRight = true;
    public float TiltSpeed = 1f;
    [Range(0, 360)]
    public float Tiltmagnitude = 10;
    public float ClosingSpeed = .2f;
    public float ClosingError = 0f;
    public bool IsDead = false;

    private const int _pi = 180;
    private const int _2pi = 360;
    private SplitToPixels stp;
    private Quaternion _initRot;


    public void Awake()
    {
        stp = GetComponent<SplitToPixels>();
        _initRot = transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Invoke(nameof(Die), 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead) return;

        if (RotateRight)
        {
            transform.Rotate(0, 0, TiltSpeed * Time.deltaTime);

            if (transform.rotation.eulerAngles.z >= Tiltmagnitude && transform.rotation.eulerAngles.z < _pi)
            {
                RotateRight = false;
            }
        } else
        {
            transform.Rotate(0, 0, -TiltSpeed * Time.deltaTime);

            //Debug.Log(transform.rotation.eulerAngles.z);

            if (transform.rotation.eulerAngles.z < _2pi - Tiltmagnitude && transform.rotation.eulerAngles.z > _pi)
            {
                RotateRight = true;
            }
        }

        if (target)
        {
            Collider2D c2d = target.GetComponent<Collider2D>();
            Collider2D c2dEnemy = GetComponent<Collider2D>();
            bool targetReachedX = Mathf.Abs(target.transform.position.x - transform.position.x) <= c2d.bounds.extents.x + c2dEnemy.bounds.extents.x + ClosingError;
            bool targetReachedY = Mathf.Abs(target.transform.position.y - transform.position.y) <= c2d.bounds.extents.y + c2dEnemy.bounds.extents.y + ClosingError;

            //Debug.Log(Mathf.Abs(target.transform.position.x - transform.position.x) + " " + (c2d.bounds.extents.x + c2dEnemy.bounds.extents.x + ClosingError));
            if (!(targetReachedX && targetReachedY))
            {
                //Debug.Log("Target not reached");
                transform.position += (target.transform.position - transform.position).normalized * ClosingSpeed * Time.deltaTime;
            }
        }
    }

    public void Die()
    {
        if (IsDead) return;

        IsDead = true;
        transform.rotation = _initRot;

        stp.SplitByPixel();
        stp.ToAshes();
    }
}
