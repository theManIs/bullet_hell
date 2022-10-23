using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class QiangPoke : MeleeWeaponFrame
{
    public override WeaponFrame GetAsset() => GameAssets.QiangPoke;

    public float MovementStage = 0;
    // public Vector3 EnemyPosition;
    // public Vector3 StartPosition;
    public float HalfEffectDuration => EffectDuration / 2;

    public override void Start()
    {
        base.Start();
        
        DirectionVector3 = EnemyPosition - transform.position;
        transform.rotation = CalculateRotation(DirectionVector3);
        
        DestroyIfNoEnemy();
        Destroy(gameObject, EffectDuration);
    }

    public void Update()
    {
        // if (Mathf.Abs(Vector3.Distance(transform.position, StartPosition)) <= WeaponRange)
        // {
        //     transform.position += DirectionVector3 * Time.deltaTime * FlightSpeed * (HalfEffectDuration - MovementStage);
        // }

        // DirectionVector3 = EnemyPosition - transform.position;
        transform.position += DirectionVector3 * Time.deltaTime * FlightSpeed * (HalfEffectDuration - MovementStage);

        MovementStage += Time.deltaTime;
        transform.rotation = CalculateRotation(DirectionVector3);

        // if (MovementStage >= EffectDuration)
        // {
        //     Destroy(gameObject);
        // }
    }

    // public override void Setup()
    // {
    //     Vector3 enemyPosition = PickEnemy(FetchVector3FromEnemyCoxswain(), TransformOfOrigin.position);
    //     
    //         // DebugEx.LogList(new List<Vector3>(FetchVector3FromEnemyCoxswain()));
    //     if (enemyPosition != Vector3.zero)
    //     {
    //         // Debug.Log(enemyPosition + " " + Vector3.Distance(TransformOfOrigin.position, enemyPosition));
    //         if (Instantiate(GetAsset(), TransformOfOrigin) is QiangPoke qiangPoke)
    //         {
    //             // qiangPoke.StartPosition = qiangPoke.transform.position;
    //             qiangPoke.EnemyPosition = enemyPosition;
    //             // qiangPoke.DirectionVector3 = enemyPosition - qiangPoke.transform.position;
    //             // qiangPoke.transform.rotation = CalculateRotation(qiangPoke.DirectionVector3);
    //         
    //             // Destroy(qiangPoke.gameObject, EffectDuration);
    //         }
    //     }
    // }

    // public void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawLine(transform.position, EnemyPosition);
    // }


    // public void OnTriggerEnter2D(Collider2D collision)
    // {
    //     DestroyOnContact(collision.gameObject, EffectDuration);
    // }
}
