using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Rename into PlatformManager
//TODO: Have the Factory reference be handled inside this class
public class PlatformPoolManager
{
    private Dictionary<string, ObjectPool> platformPools;

    public PlatformPoolManager(Dictionary<string, GameObject> prefabs, int initialSize, Transform parent = null)
    {
        platformPools = new Dictionary<string, ObjectPool>();

        foreach (var kvp in prefabs)
        {
            platformPools[kvp.Key] = new ObjectPool(kvp.Value, initialSize, parent);
        }
    }

    public GameObject GetPlatform(string platformType, Vector3 position)
    {
        if (platformPools.ContainsKey(platformType))
        {
            return platformPools[platformType].GetObject(position);
        }

        Debug.LogError($"Platform type {platformType} not found in pool.");
        return null;
    }

    public void ReturnPlatform(GameObject platform)
    {
        string platformType = platform.GetComponent<Platform>().GetType().Name;
        if (platformPools.ContainsKey(platformType))
        {
            platformPools[platformType].ReturnObject(platform);
        }
        else
        {
            Debug.LogError($"Platform type {platformType} not found in pool.");
        }
    }
}
