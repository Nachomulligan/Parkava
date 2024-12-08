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

    [Header("Platform Scale Settings")]
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public float scaleStep = 0.1f;

    private IPlatformService platformService;

    [Header("Platform Direction")]
    public Vector3 direction = Vector3.right;

    [Header("Pooling Settings")]
    public int initialPoolSize = 10;

    private void Start()
    {
        platformService = ServiceLocator.Instance.GetService(nameof(IPlatformService)) as IPlatformService;

        if (platformService == null)
        {
            Debug.LogError("PlatformService not found in ServiceLocator!");
            return;
        }

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
        GameObject platformObject = platformService.GetPlatform(spawnPosition);

        if (platformObject != null)
        {
            Vector3 normalizedDirection = direction.normalized;

            Platform platformComponent = platformObject.GetComponent<Platform>();
            platformComponent.Initialize(speed, normalizedDirection);

            StartCoroutine(DeactivateAfterTime(platformObject, platformLifetime));
        }
    }

    private IEnumerator DeactivateAfterTime(GameObject platform, float time)
    {
        yield return new WaitForSeconds(time);
        platformService.ReturnPlatform(platform);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawSphere(transform.position, 2f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction.normalized * 2f);
    }
}
