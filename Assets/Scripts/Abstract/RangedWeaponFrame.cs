using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class RangedWeaponFrame : WeaponFrame
{
    public string IgnoreDestroyLayer = "Player";
    public string WeaponLayer = "DamageEnemy";
    public Vector3 DirectionVector3;
    public float Lifespan = 3f;
    public float ProjectileLife = 0;
    public bool Go = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(WeaponLayer);
        GetComponent<Collider2D>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Go)
        // {
            Debug.Log(gameObject.name + " " + GetInstanceID());
            transform.position += DirectionVector3 * Time.deltaTime * FlightSpeed;
            ProjectileLife += Time.deltaTime;

            if (ProjectileLife >= Lifespan)
            {
                Destroy(gameObject);
            }
        // }


        /*if (transform.position == TargetPosition)
        {
            Destroy(gameObject);
        }*/
    }

    public override WeaponFrame Launch(Transform transformOfOrigin)
    {
        TransformOfOrigin = transformOfOrigin;
        // Go = true;

        InvokeRepeating(nameof(GoWeaponGo), 0, WeaponCooldown);

        return this;
    }

    public void GoWeaponGo()
    {
        // if (Go)
        // {
        List<EnemyCoxswain> lec = new List<EnemyCoxswain>(FindObjectsOfType<EnemyCoxswain>());
        List<Vector3> lv3 = new List<Vector3>();

        foreach (EnemyCoxswain enemyCoxswain in lec)
        {
            if (!enemyCoxswain.IsDead)
            {
                lv3.Add(enemyCoxswain.transform.position);
            }
        }

        Setup(TransformOfOrigin.position, lv3.ToArray());
        // }
    }

    public void CalculateRotation()
    {
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, 90) * DirectionVector3;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rotatedDirection);
    }

    public WeaponFrame Setup(Vector3 position, Vector3[] targetPositions)
    {
        // Debug.Log(GetAsset());
        RangedWeaponFrame ash = (RangedWeaponFrame)Instantiate(GetAsset(), position, Quaternion.identity);

        Vector3 targetPosition =
            // new List<Vector3>(targetPositions).Find(targetPosition => Vector3.Distance(targetPosition, position) <= ash.WeaponRange);
            new List<Vector3>(targetPositions).Find(targetPosition => (targetPosition - position).sqrMagnitude <= Mathf.Pow(ash.WeaponRange, 2));

        if (targetPosition != Vector3.zero)
        {
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
