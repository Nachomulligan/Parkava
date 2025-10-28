using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformService : MonoBehaviour, IPlatformService
{
    private Dictionary<string, ObjectPool> platformPools;
    private PlatformFactory platformFactory;
    private List<string> platformTypes;
    private int currentPlatformIndex;

    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(IPlatformService), this);
    }

    public void Initialize(Dictionary<string, GameObject> prefabs, float minScale, float maxScale, float scaleStep, int initialPoolSize)
    {
        platformFactory = new PlatformFactory(new List<GameObject>(prefabs.Values), minScale, maxScale);
        platformPools = new Dictionary<string, ObjectPool>();
        platformTypes = new List<string>(prefabs.Keys);

        foreach (var kvp in prefabs)
        {
            platformPools[kvp.Key] = new ObjectPool(kvp.Value, initialPoolSize, transform);
        }

        Debug.Log($"PlatformService initialized with {platformPools.Count} platform types.");
    }

    public GameObject GetPlatform(Vector3 position, string type = null)
    {
        string selectedType = type;

        // If no type requested, use round-robin
        if (string.IsNullOrEmpty(selectedType))
        {
            selectedType = platformTypes[currentPlatformIndex];
            currentPlatformIndex = (currentPlatformIndex + 1) % platformTypes.Count;
        }

        if (platformPools.ContainsKey(selectedType))
        {
            GameObject platform = platformPools[selectedType].GetObject(position);

            if (platform == null)
            {
                platform = platformFactory.Create(position);
            }

            platformFactory.UpdateScale(platform);
            return platform;
        }

        Debug.LogError($"Platform type {selectedType} not found in pool.");
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
