using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.WSA;

[RequireComponent(typeof(Collider2D))]
public abstract class WeaponFrame : MonoBehaviour
{
    public float WeaponRange = 5f;
    public float WeaponCooldown = 2f;
    public float FlightSpeed = 5f;
    public string WeaponLayer = "DamageEnemy";
    public string EnvironmentLayer = "Environment";
    public Vector3 DirectionVector3;
    public string IgnoreDestroyLayer = "Player";

    protected Transform TransformOfOrigin;

    public virtual void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(WeaponLayer);
        GetComponent<Collider2D>().isTrigger = true;
    }

    public abstract WeaponFrame Launch(Transform transformOfOrigin);
    public abstract WeaponFrame GetAsset();

    public Quaternion CalculateRotation(Vector3 direction)
    {
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, 90) * direction;
        
        return Quaternion.LookRotation(Vector3.forward, rotatedDirection);
    }

    public virtual Vector3 PickEnemy(Vector3[] targetPositions, Vector3 sourcePosition)
    {
        Vector3 targetPosition =
            // new List<Vector3>(targetPositions).Find(targetPosition => Vector3.Distance(targetPosition, sourcePosition) <= WeaponRange);
            new List<Vector3>(targetPositions).Find(targetPosition => (targetPosition - sourcePosition).sqrMagnitude <= Mathf.Pow(WeaponRange, 2));
            // Debug.Log(sourcePosition + " " + Vector3.Distance(targetPosition, sourcePosition));
            // DebugEx.LogList(new List<Vector3>(targetPositions));
        return targetPosition;
    }

    public virtual Vector3[] FetchVector3FromEnemyCoxswain()
    {
        EnemyCoxswain[] eCox = FindObjectsOfType<EnemyCoxswain>();
        List<Vector3> v3s = new List<Vector3>();

        foreach (EnemyCoxswain enemyCoxswain in eCox)
        {
            if (!enemyCoxswain.IsDead)
            {
                v3s.Add(enemyCoxswain.transform.position);
            }
        }

        return v3s.ToArray();
    }

    public virtual void MoveForward()
    {
        transform.position += DirectionVector3 * Time.deltaTime * FlightSpeed;
    }

    public virtual void DestroyOnContact(GameObject gm, float after = 0)
    {
        if (gm.layer != LayerMask.NameToLayer(IgnoreDestroyLayer))
        {
            Destroy(gameObject, after);
        }
    }

    public bool IgnorePlayerL(GameObject gm) => gm.layer != LayerMask.NameToLayer(IgnoreDestroyLayer);
    public bool IgnoreEnvironmentL(GameObject gm) => gm.layer != LayerMask.NameToLayer(EnvironmentLayer);
    public bool IgnoreEnvPl(GameObject gm) => IgnorePlayerL(gm) && IgnoreEnvironmentL(gm);
}
