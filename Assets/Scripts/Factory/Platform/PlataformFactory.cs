
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFactory : AbstractFactory<GameObject>
{
    private Dictionary<string, GameObject> prefabs;
    private float currentScale;

    public PlatformFactory(Dictionary<string, GameObject> prefabs, float minScale, float maxScale, float scaleStep)
        : base(minScale, maxScale, scaleStep)
    {
        this.prefabs = prefabs;
        currentScale = minScale;
    }
    public override GameObject Create(Vector3 position, string platformType)
    {
        if (!prefabs.ContainsKey(platformType))
        {
            Debug.LogError($"Platform type {platformType} not found in factory.");
            return null;
        }

        GameObject platform = GameObject.Instantiate(prefabs[platformType], position, Quaternion.identity);
        UpdateScale(platform);
        return platform;
    }

    public override GameObject Create(Vector3 position)
    {
        Debug.LogError("This method is not implemented in PlatformFactory.");
        return null;
    }

    public override void UpdateScale(GameObject platform)
    {
        Vector3 originalScale = platform.transform.localScale;
        float randomScale = Random.Range(minScale, maxScale);
        platform.transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z * randomScale);
    }
}
