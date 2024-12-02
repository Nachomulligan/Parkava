using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : IBulletFactory
{
    private GameObject bulletPrefab;
    private float currentScale;

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
        UpdateBulletColor(bulletObject);
        return bulletObject;
    }

    public void UpdateBulletScale(GameObject bulletObject)
    {
        float randomScale = Random.Range(minScale, maxScale);
        bulletObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    public void UpdateBulletColor(GameObject bulletObject)
    {
        Renderer renderer = bulletObject.GetComponent<Renderer>();
        if (renderer != null && renderer.material.HasProperty("_EmissionColor"))
        {
            Color randomEmissionColor = new Color(Random.value, Random.value, Random.value);
            renderer.material.SetColor("_EmissionColor", randomEmissionColor);

            renderer.material.EnableKeyword("_EMISSION");
        }
    }
}
