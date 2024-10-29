using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFactory : IPlatformFactory
{
    private ObjectPool platformPool;

    public PlatformFactory(GameObject prefab, int initialSize, Transform parent = null)
    {
        platformPool = new ObjectPool(prefab, initialSize, parent);
    }

    public GameObject CreatePlatform(Vector3 position)
    {
        return platformPool.GetObject(position);
    }

    public void ReturnPlatform(GameObject platform)
    {
        platformPool.ReturnObject(platform);
    }
}
