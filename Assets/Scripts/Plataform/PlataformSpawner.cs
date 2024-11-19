﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject linePlatformPrefab;
    public GameObject destructiblePlatformPrefab;
    public Vector3 spawnPosition;
    public float spawnInterval = 2f;
    public float speed = 2f;
    public float platformLifetime = 5f;

    [Header("Platform Scale Settings")]
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public float scaleStep = 0.1f;
    public int initialPoolSize = 10;

    private IPlatformService platformService;
    private string[] platformTypes = { "LinePlatform", "DestructibleMovingPlatform" };
    private int currentPlatformIndex = 0;

    private void Start()
    {
        platformService = ServiceLocator.Instance.GetService(nameof(IPlatformService)) as IPlatformService;

        if (platformService == null)
        {
            Debug.LogError("PlatformService not found in ServiceLocator!");
            return;
        }

        // Inicializar PlatformService con los datos del spawner
        var prefabs = new Dictionary<string, GameObject>
        {
            { "LinePlatform", linePlatformPrefab },
            { "DestructibleMovingPlatform", destructiblePlatformPrefab }
        };

        platformService.Initialize(prefabs, minScale, maxScale, scaleStep, initialPoolSize);

        InvokeRepeating(nameof(SpawnPlatform), 0f, spawnInterval);
    }

    private void SpawnPlatform()
    {
        string platformType = platformTypes[currentPlatformIndex];
        currentPlatformIndex = (currentPlatformIndex + 1) % platformTypes.Length;

        GameObject platformObject = platformService.GetPlatform(spawnPosition, platformType);

        if (platformObject != null)
        {
            Platform platformComponent = platformObject.GetComponent<Platform>();
            platformComponent.Initialize(speed);
            StartCoroutine(DeactivateAfterTime(platformObject, platformLifetime));
        }
    }

    private IEnumerator DeactivateAfterTime(GameObject platform, float time)
    {
        yield return new WaitForSeconds(time);
        platformService.ReturnPlatform(platform);
    }
}
