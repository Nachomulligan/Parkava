using System.Collections;
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

    private IPlatformFactory platformFactory;
    private string[] platformTypes = { "LinePlatform", "DestructibleMovingPlatform" };
    private int currentPlatformIndex = 0;

    private void Start()
    {
        var platformPrefabs = new Dictionary<string, GameObject>
        {
            { "LinePlatform", linePlatformPrefab },
            { "DestructibleMovingPlatform", destructiblePlatformPrefab }
        };

        platformFactory = new PlatformFactory(platformPrefabs, 10, transform);
        InvokeRepeating(nameof(SpawnPlatform), 0f, spawnInterval);
    }

    private void SpawnPlatform()
    {
        string platformType = platformTypes[currentPlatformIndex];
        currentPlatformIndex = (currentPlatformIndex + 1) % platformTypes.Length;

        GameObject platformObject = platformFactory.CreatePlatform(spawnPosition, platformType);

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
        platformFactory.ReturnPlatform(platform);
    }
}
