using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletMovementStrategy : ScriptableObject
{
    public abstract void Initialize(Vector3 targetPosition, Vector3 startPosition);
    public abstract void Move(Transform bulletTransform, Vector3 targetPosition);
}
