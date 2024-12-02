using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : AbstractFactory<GameObject>
{
    private GameObject bulletPrefab;

    public BulletFactory(GameObject bulletPrefab, float minScale, float maxScale, float scaleStep)
        : base(minScale, maxScale, scaleStep)
    {
        this.bulletPrefab = bulletPrefab;
    }

    public override GameObject Create(Vector3 position)
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, position, Quaternion.identity);
        UpdateScale(bullet);
        UpdateColor(bullet);
        return bullet;
    }

    public override void UpdateScale(GameObject bullet)
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