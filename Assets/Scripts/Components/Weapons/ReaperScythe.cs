using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ReaperScythe : MeleeWeaponFrame
{
    [Range(0, 89)]
    public float RotationAngle = 45;

    // public float AnimationDuration = .5f;
    //
    // public Color EffectColor = Color.red;

    public override WeaponFrame GetAsset() => GameAssets.ReaperScythe;

    public override void Start()
    {
        base.Start();
        // Debug.Log(TransformOfOrigin.name);
        // bool flipX = TransformOfOrigin.GetComponent<SpriteRenderer>().flipX;
        // LastAxis = flipX ? 1 : -1;
        bool flipX = LastAxis > 0;
        transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = flipX;
        
        // Debug.Log(Mathf.Rad2Deg * Mathf.PI);
        transform.rotation = Quaternion.Euler(0, 0, flipX ? Mathf.Rad2Deg * Mathf.PI - RotationAngle : RotationAngle);

        // float h = Input.GetAxis("Horizontal");
        //
        // if (h != 0)
        // {
        //     LastAxis = Mathf.Sign(Input.GetAxis("Horizontal"));
        // }
        //
        // transform.rotation = Quaternion.Euler(0, 0, LastAxis > 0 ? RotationAngle + 180 : RotationAngle);
        // transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = LastAxis > 0;
        //
        // Vector3 scythePos = transform.GetChild(0).localPosition;
        // Vector3 endPoint = new Vector3(scythePos.x, -scythePos.y);
        // EdgeCollider2D ec2d = gameObject.AddComponent<EdgeCollider2D>();
        // ec2d.isTrigger = true;
        // ec2d.points = new Vector2[4] { Vector3.zero, scythePos, endPoint, Vector3.zero};

        RotationAngle = Mathf.Clamp(RotationAngle, 0, 89);
        
        // DestroyIfNoEnemy();
    }

    public void Update()
    {
        transform.Rotate(Vector3.forward, LastAxis * FlightSpeed * Time.deltaTime);

        if (transform.rotation.eulerAngles.z > Mathf.Rad2Deg * Mathf.PI + RotationAngle && transform.rotation.eulerAngles.z < 360 - RotationAngle)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (IgnoreEnvPlCl(collision.gameObject))
        {
            // Debug.Log(collision.name);

            AddTimedDamage apd = AddTimedDamage.Setup<AddBloodDamage>(collision.gameObject);
        }
    }
}
