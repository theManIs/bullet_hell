using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public abstract class WeaponFrame : MonoBehaviour
{
    public float WeaponRange = 5f;
    public float WeaponCooldown = 2f;
    public float FlightSpeed = 5f;

    protected Transform TransformOfOrigin;

    public abstract WeaponFrame Launch(Transform transformOfOrigin);
    public abstract WeaponFrame GetAsset();
}
