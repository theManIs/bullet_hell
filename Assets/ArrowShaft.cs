using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShaft : WeaponFrame
{
    public float WeaponRange = 5f;
    public float WeaponCooldown = 2f;
    public float FlightSpeed = 5f;

    public string IgnoreDestroyLayer = "Player";
    public Vector3 TargetPosition;
    public Vector3 DirectionVector3;
    public float Lifespan = 3f;
    public float ProjectileLife = 0;
    public bool Go = false;

    private Transform _transformOfOrigin;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Go)
        {
            transform.position += DirectionVector3 * Time.deltaTime * FlightSpeed;
            ProjectileLife += Time.deltaTime;

            if (ProjectileLife >= Lifespan)
            {
                Destroy(gameObject);
            }
        }
        

        /*if (transform.position == TargetPosition)
        {
            Destroy(gameObject);
        }*/
    }

    public ArrowShaft SetTransformOfOrigin(Transform transformOfOrigin)
    {
        _transformOfOrigin = transformOfOrigin;
        Go = true;

        InvokeRepeating(nameof(GoWeaponGo), 0, WeaponCooldown);

        return this;
    }

    public void GoWeaponGo()
    {
        if (Go)
        {
            List<EnemyCoxswain> lec = new List<EnemyCoxswain>(FindObjectsOfType<EnemyCoxswain>());
            List<Vector3> lv3 = new List<Vector3>();

            foreach (EnemyCoxswain enemyCoxswain in lec)
            {
                if (!enemyCoxswain.IsDead)
                {
                    lv3.Add(enemyCoxswain.transform.position);
                }
            }

            Setup(_transformOfOrigin.position, lv3.ToArray());
        }
    }

    public void CalculateRotation()
    {
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, 90) * DirectionVector3;
         transform.rotation = Quaternion.LookRotation( Vector3.forward, rotatedDirection);
    }

    public static ArrowShaft GetArrowShaftAsset() => GameAssets.ArrowShaft;

    public static ArrowShaft Setup(Vector3 position, Vector3[] targetPositions)
    {
        ArrowShaft ash = Instantiate(GameAssets.ArrowShaft, position, Quaternion.identity);

        Vector3 targetPosition =
            // new List<Vector3>(targetPositions).Find(targetPosition => Vector3.Distance(targetPosition, position) <= ash.WeaponRange);
            new List<Vector3>(targetPositions).Find(targetPosition =>  (targetPosition - position).sqrMagnitude <= Mathf.Pow(ash.WeaponRange, 2));

        if (targetPosition != Vector3.zero)
        {
            ash.TargetPosition = targetPosition;
            ash.DirectionVector3 = (targetPosition - ash.transform.position).normalized;

            //ash.transform.rotation.SetFromToRotation(Vector3.right, targetPosition);
            ash.CalculateRotation();
        }
        else
        {
            Destroy(ash.gameObject);
        }

        return ash;
    }

    // public IEnumerable ShootWithArrows()
    // {
    //     yield return new WaitForSeconds(WeaponCooldown);
    //
    //     List<EnemyCoxswain> lec = new List<EnemyCoxswain>(FindObjectsOfType<EnemyCoxswain>());
    //
    //     ArrowShaft.Setup(_transformOfOrigin.position, lec[0].transform.position, _transformOfOrigin);
    //
    //     yield return ;
    // }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(IgnoreDestroyLayer))
        {
            Destroy(gameObject);
        }
    }
}
