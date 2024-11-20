using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlatformService : MonoBehaviour
{
    private List<DestructiblePlatform> destructiblePlatforms = new();
    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(DestructiblePlatformService), this);
    }

    public void RegisterPlatform(DestructiblePlatform platform)
    {
        if (!destructiblePlatforms.Contains(platform))
        {
            destructiblePlatforms.Add(platform);
        }
    }

    public void UnregisterPlatform(DestructiblePlatform platform)
    {
        destructiblePlatforms.Remove(platform);
    }

    public void ReactivateAllPlatforms()
    {
        foreach (var platform in destructiblePlatforms)
        {
            platform.Reactivate();
        }
    }
}