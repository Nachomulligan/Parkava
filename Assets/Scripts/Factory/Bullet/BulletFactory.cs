using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : IBulletFactory
{
    private GameObject bulletPrefab;
    private float currentScale;
    private bool scalingUp = true;

    private float minScale;
    private float maxScale;
    private float scaleStep;

    public BulletFactory(GameObject bulletPrefab, float minScale, float maxScale, float scaleStep)
    {
        this.bulletPrefab = bulletPrefab;
        this.minScale = minScale;
        this.maxScale = maxScale;
        this.scaleStep = scaleStep;
        currentScale = minScale;
    }

    public GameObject CreateBullet(Vector3 position)
    {
        GameObject bulletObject = GameObject.Instantiate(bulletPrefab, position, Quaternion.identity);
        UpdateBulletScale(bulletObject);
        return bulletObject;
    }

    public void UpdateBulletScale(GameObject bulletObject)
    {
        float randomScale = Random.Range(minScale, maxScale);
        bulletObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}
