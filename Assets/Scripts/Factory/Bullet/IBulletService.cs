using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBulletService
{
    GameObject GetBullet(Vector3 position);
    void Initialize(GameObject bulletPrefab, float minScale, float maxScale, float scaleStep, int initialPoolSize);
    void ReturnBullet(GameObject bullet);
}
