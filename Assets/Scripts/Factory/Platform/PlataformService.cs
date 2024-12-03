using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformService : MonoBehaviour, IPlatformService
{
    private PlatformFactory platformFactory;

    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(IPlatformService), this);
    }

    public void Initialize(List<GameObject> prefabs, float minScale, float maxScale, float scaleStep)
    {
        platformFactory = new PlatformFactory(prefabs, minScale, maxScale, scaleStep);
        Debug.Log($"PlatformService initialized with: MinScale={minScale}, MaxScale={maxScale}, ScaleStep={scaleStep}");
    }

    public GameObject GetPlatform(Vector3 position)
    {
        GameObject platform = platformFactory.Create(position);
        platformFactory.UpdateScale(platform);
        return platform;
    }

    public void ReturnPlatform(GameObject platform)
    {
        platformFactory.UpdateScale(platform);
    }
}