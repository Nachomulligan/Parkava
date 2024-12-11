using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : AbstractFactory<GameObject>, IScalableFactory<GameObject>
{
    private GameObject bulletPrefab;
    private float minScale;
    private float maxScale;

    public BulletFactory(GameObject bulletPrefab, float minScale, float maxScale)
    {
        this.bulletPrefab = bulletPrefab;
        this.minScale = minScale;
        this.maxScale = maxScale;
    }

    public override GameObject Create(Vector3 position)
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, position, Quaternion.identity);
        UpdateScale(bullet);
        return bullet;
    }

    public void UpdateScale(GameObject bullet)
    {
        float randomScale = Random.Range(minScale, maxScale);
        bullet.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    public void UpdateColor(GameObject bullet)
    {
        Renderer renderer = bullet.GetComponent<Renderer>();
        if (renderer != null && renderer.material.HasProperty("_EmissionColor"))
        {
            Color randomEmissionColor = new Color(Random.value, Random.value, Random.value);
            renderer.material.SetColor("_EmissionColor", randomEmissionColor);
            renderer.material.EnableKeyword("_EMISSION");
        }
    }
}