using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;     
    public Vector3 spawnPosition;         
    public float spawnInterval = 2f;       
    public float speed = 2f;              
    public float platformLifetime = 5f;   

    private IPlatformFactory platformFactory;

    private void Start()
    {
        platformFactory = new PlatformFactory(platformPrefab, 10, transform);
        InvokeRepeating(nameof(SpawnPlatform), 0f, spawnInterval);
    }

    private void SpawnPlatform()
    {
        // Pide una plataforma al pool usando la factory
        GameObject platform = platformFactory.CreatePlatform(spawnPosition);

        var listPlatform = platform.GetComponent<LinePlataform>();
        if (listPlatform == null)
        {
            listPlatform = platform.AddComponent<LinePlataform>();
        }
        listPlatform.Initialize(speed);
        StartCoroutine(DeactivateAfterTime(platform, platformLifetime));
    }

    private IEnumerator DeactivateAfterTime(GameObject platform, float time)
    {
        yield return new WaitForSeconds(time);

        platformFactory.ReturnPlatform(platform);
    }
}
