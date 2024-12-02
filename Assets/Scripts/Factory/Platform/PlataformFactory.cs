
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFactory : AbstractFactory<GameObject>
{
    private List<GameObject> prefabs;
    private float currentScale;
    private int currentPrefabIndex;

    public PlatformFactory(List<GameObject> prefabs, float minScale, float maxScale, float scaleStep)
        : base(minScale, maxScale, scaleStep)
    {
        this.prefabs = prefabs;
        currentScale = minScale;
        currentPrefabIndex = 0;
    }

    public override GameObject Create(Vector3 position)
    {
        if (prefabs.Count == 0)
        {
            Debug.LogError("No prefabs available in PlatformFactory.");
            return null;
        }

        GameObject prefabToInstantiate = prefabs[currentPrefabIndex];
        currentPrefabIndex = (currentPrefabIndex + 1) % prefabs.Count;

        GameObject platform = GameObject.Instantiate(prefabToInstantiate, position, Quaternion.identity);
        UpdateScale(platform);
        return platform;
    }

    public override void UpdateScale(GameObject platform)
    {
        Vector3 originalScale = platform.transform.localScale;
        float randomScale = Random.Range(minScale, maxScale);
        platform.transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z * randomScale);
    }
}
