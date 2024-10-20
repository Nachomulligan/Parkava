using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponStrategy : ScriptableObject
{
    public abstract void Shoot(Transform shootPoint, GameObject bulletPrefab, Transform target);
}
