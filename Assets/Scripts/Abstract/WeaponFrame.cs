using System.Collections.Generic;
using Assets.Scripts.Types;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class WeaponFrame : MonoBehaviour
{
    public float WeaponRange = 5f;
    public float WeaponCooldown = 2f;
    public float FlightSpeed = 5f;
    public string WeaponLayer = LayerList.DamageEnemy.ToString();
    public string EnvironmentLayer = LayerList.Environment.ToString();
    public string CollectiblesLayer = LayerList.Collectibles.ToString();
    public Vector3 DirectionVector3;
    public string PlayerLayer = LayerList.Player.ToString();
    public Vector3 EnemyPosition;
    public Transform TransformOfOrigin;
    public int LastAxis = -1;
    public List<int> IgnoreList = new List<int>();

    public virtual void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(WeaponLayer);
        GetComponent<Collider2D>().isTrigger = true;
        DirectionVector3 = (EnemyPosition - transform.position).normalized;
    }

    public abstract WeaponFrame GetAsset();

    public virtual WeaponFrame Launch(Transform transformOfOrigin)
    {
        TransformOfOrigin = transformOfOrigin;

        InvokeRepeating(nameof(Setup), 0, WeaponCooldown);

        return this;
    }

    public virtual WeaponFrame Setup() => Setup(Instantiate(GetAsset(), TransformOfOrigin));

    public virtual WeaponFrame Setup(WeaponFrame wpf)
    {
        if (wpf is { } rs)
        {
            rs.TransformOfOrigin = TransformOfOrigin;
            rs.LastAxis = TransformOfOrigin.GetComponent<KnightCoxswain>().LastDirectionLeft ? 1 : -1;
            // rs.EnemyPosition = PickEnemy(FetchVector3FromEnemyCoxswain(ignoreList), TransformOfOrigin.position, weaponRange);
        }

        return wpf;
    }

    public Vector3 FindEnemyPosition(float weaponRange, List<int> ignoreList)
    {
        if (TransformOfOrigin)
        {
            return PickEnemy(FetchVector3FromEnemyCoxswain(ignoreList), TransformOfOrigin.position, weaponRange);
        }
        else
        {
            return PickEnemy(FetchVector3FromEnemyCoxswain(ignoreList), Vector3.zero, weaponRange);
        }
    }

    public Quaternion CalculateRotation(Vector3 direction)
    {
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, 90) * direction;
        
        return Quaternion.LookRotation(Vector3.forward, rotatedDirection);
    }

    public virtual Vector3 PickEnemy(Vector3[] targetPositions, Vector3 sourcePosition) => PickEnemy(targetPositions, sourcePosition, WeaponRange);
    
    public virtual Vector3 PickEnemy(Vector3[] targetPositions, Vector3 sourcePosition, float customRange)
    {
        // foreach (Vector3 vector3 in targetPositions)
        // {
        //     Debug.Log(sourcePosition + " " + vector3 + " " + Vector3.Distance(vector3, sourcePosition) + " < " + customRange
        //               + " magnitude " + (vector3 - sourcePosition).sqrMagnitude + " < " + Mathf.Pow(customRange, 2));
        // }

        Vector3 targetPosition =
            // new List<Vector3>(targetPositions).Find(targetPosition => Vector3.Distance(targetPosition, sourcePosition) <= customRange);
            new List<Vector3>(targetPositions).Find(targetPosition => (targetPosition - sourcePosition).sqrMagnitude <= Mathf.Pow(customRange, 2));
            // Debug.Log(sourcePosition + " " + Vector3.Distance(targetPosition, sourcePosition) + " < " + customRange);
            // DebugEx.LogList(new List<Vector3>(targetPositions));
            // print(targetPosition);
        return targetPosition;
    }

    public virtual Vector3[] FetchVector3FromEnemyCoxswain(List<int> ignoreList)
    {
        EnemyCoxswain[] eCox = FindObjectsOfType<EnemyCoxswain>();
        List<Vector3> v3s = new List<Vector3>();

        foreach (EnemyCoxswain enemyCoxswain in eCox)
        {
            if (ignoreList == null || !ignoreList.Contains(enemyCoxswain.gameObject.GetInstanceID()))
            {
                if (!enemyCoxswain.IsDead)
                {
                    v3s.Add(enemyCoxswain.transform.position);
                }
            }
        }

        return v3s.ToArray();
    }

    public virtual void MoveForward()
    {
        transform.position += DirectionVector3 * Time.deltaTime * FlightSpeed;
    }

    // public virtual void DestroyOnContact(GameObject gm, float after = 0)
    // {
    //     if (gm.layer != LayerMask.NameToLayer(IgnoreDestroyLayer))
    //     {
    //         Destroy(gameObject, after);
    //     }
    // }

    public bool IgnorePlayerL(GameObject gm) => gm.layer != LayerMask.NameToLayer(PlayerLayer);
    public bool IgnoreEnvironmentL(GameObject gm) => gm.layer != LayerMask.NameToLayer(EnvironmentLayer);
    public bool IgnoreCollectiblesL(GameObject gm) => gm.layer != LayerMask.NameToLayer(CollectiblesLayer);
    public bool IgnoreEnvPl(GameObject gm) => IgnorePlayerL(gm) && IgnoreEnvironmentL(gm);
    public bool IgnoreEnvPlCl(GameObject gm) => IgnorePlayerL(gm) && IgnoreEnvironmentL(gm) && IgnoreCollectiblesL(gm);
}
