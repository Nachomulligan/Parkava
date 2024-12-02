using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBulletFactory
{
    GameObject CreateBullet(Vector3 position);
    void UpdateBulletScale(GameObject bulletObject);
}
