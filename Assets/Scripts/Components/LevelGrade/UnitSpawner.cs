using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject[] UnitsList;
    public Camera HudCamera;
    // public bool Instant;
    public float LockTimer = 0;
    public float SpawnSpeed = 1f;
    public GameObject Player;
    public bool IsUpdate = false;
    public LevelTimer LevelTimer;

    private List<GameObject> _lgo = new List<GameObject>();

    public void Awake()
    {
        LevelTimer = FindObjectOfType<LevelTimer>();
        FindObjectOfType<PickingCharacterScreen>().OnGo += () => gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    public void Start()
    {
        gameObject.SetActive(false);
        LevelTimer.OnTimeUp += () => gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        Player = FindObjectOfType<KnightCoxswain>().gameObject;
        IsUpdate = true;
    }

    public void OnDisable()
    {
        _lgo.ForEach(Destroy);
        IsUpdate = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (LockTimer <= 0)
        {
            bool xOrY = Random.value >= .5f;
            bool forwardOrBackward = Random.value >= .5f;
            float xPoint, yPoint;

            if (xOrY)
            {
                xPoint = Random.Range(0f, 1f);
                yPoint = forwardOrBackward ? -.1f : 1.1f;
            }
            else
            {
                xPoint = forwardOrBackward ? -.1f : 1.1f;
                yPoint = Random.Range(0f, 1f);
            }

            Vector3 pointOfTouch = HudCamera.ViewportToWorldPoint(new Vector3(xPoint, yPoint));
            pointOfTouch.z = 0;
            // GameObject gm = new GameObject(pointOfTouch.ToString());
            // gm.transform.position = pointOfTouch;
            GameObject newEnemy = Instantiate(UnitsList[Random.Range(0, UnitsList.Length)], pointOfTouch, Quaternion.identity);
            newEnemy.GetComponent<EnemyCoxswain>().target = Player;
            _lgo.Add(newEnemy);

            LockTimer = SpawnSpeed;
        }

        LockTimer -= Time.deltaTime;

        // if (!Instant)
        // {
        //     Vector3 pointOfTouch = HudCamera.ViewportToWorldPoint(new Vector3(1, 1));
        //     GameObject gm = new GameObject(pointOfTouch.ToString());
        //     gm.transform.position = pointOfTouch;
        //
        //     pointOfTouch = HudCamera.ViewportToWorldPoint(new Vector3(.5f, .5f));
        //     gm = new GameObject(pointOfTouch.ToString());
        //     gm.transform.position = pointOfTouch;
        //
        //     pointOfTouch = HudCamera.ViewportToWorldPoint(new Vector3(0, 0));
        //     gm = new GameObject(pointOfTouch.ToString());
        //     gm.transform.position = pointOfTouch;
        //
        //
        //     Instant = true;
        // }
    }
}
