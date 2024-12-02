using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletService : MonoBehaviour, IBulletService
{
    private ObjectPool bulletPool;
    private BulletFactory bulletFactory;

    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(IBulletService), this);
    }

    public void Initialize(GameObject bulletPrefab, float minScale, float maxScale, float scaleStep, int initialPoolSize)
    {
        bulletFactory = new BulletFactory(bulletPrefab, minScale, maxScale, scaleStep);
        bulletPool = new ObjectPool(bulletPrefab, initialPoolSize, transform);
        Debug.Log($"BulletService initialized with: MinScale={minScale}, MaxScale={maxScale}, ScaleStep={scaleStep}");
    }

    public GameObject GetBullet(Vector3 position)
    {
        GameObject bullet = bulletPool.GetObject(position);

        if (bullet == null)
        {
            bullet = bulletFactory.Create(position);
        }
        bulletFactory.UpdateScale(bullet);
        bulletFactory.UpdateColor(bullet);

        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bulletPool.ReturnObject(bullet);
    }
}
