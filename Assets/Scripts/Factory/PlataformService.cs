﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformService : MonoBehaviour, IPlatformService
{
    private Dictionary<string, ObjectPool> platformPools;
    private IPlatformFactory platformFactory;

    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(IPlatformService), this);
    }

    public void Initialize(Dictionary<string, GameObject> prefabs, float minScale, float maxScale, float scaleStep, int initialPoolSize)
    {
        platformFactory = new PlatformFactory(prefabs, minScale, maxScale, scaleStep);
        platformPools = new Dictionary<string, ObjectPool>();

        foreach (var kvp in prefabs)
        {
            platformPools[kvp.Key] = new ObjectPool(kvp.Value, initialPoolSize, transform);
        }
        Debug.Log($"PlatformService initialized with: MinScale={minScale}, MaxScale={maxScale}, ScaleStep={scaleStep}");
    }

    public GameObject GetPlatform(Vector3 position, string platformType)
    {
        if (platformPools.ContainsKey(platformType))
        {
            GameObject platform = platformPools[platformType].GetObject(position);

            if (platform == null)
            {
                platform = platformFactory.CreatePlatform(position, platformType);
            }

            return platform;
        }

        Debug.LogError($"Platform type {platformType} not found in pool.");
        return null;
    }

    public void ReturnPlatform(GameObject platform)
    {
        string platformType = platform.GetComponent<Platform>().GetType().Name;
        if (platformPools.ContainsKey(platformType))
        {
            // Actualiza la escala antes de devolverla al pool
            platformFactory.UpdatePlatformScale(platform);
            platformPools[platformType].ReturnObject(platform);
        }
        else
        {
            Debug.LogError($"Platform type {platformType} not found in pool.");
        }
    }
}
