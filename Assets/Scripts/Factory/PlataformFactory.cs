using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFactory : IPlatformFactory
{
    private Dictionary<string, GameObject> prefabs;
    private float currentScale;
    private bool scalingUp = true;

    private float minScale;
    private float maxScale;
    private float scaleStep;

    public PlatformFactory(Dictionary<string, GameObject> prefabs, float minScale, float maxScale, float scaleStep)
    {
        this.prefabs = prefabs;
        this.minScale = minScale;
        this.maxScale = maxScale;
        this.scaleStep = scaleStep;
        currentScale = minScale;
    }

    public GameObject CreatePlatform(Vector3 position, string platformType)
    {
        if (!prefabs.ContainsKey(platformType))
        {
            Debug.LogError($"Platform type {platformType} not found in factory.");
            return null;
        }

        GameObject platformObject = GameObject.Instantiate(prefabs[platformType], position, Quaternion.identity);
        UpdatePlatformScale(platformObject);
        return platformObject;
    }

    public void UpdatePlatformScale(GameObject platformObject)
    {
        Vector3 originalScale = platformObject.transform.localScale;
        platformObject.transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z * currentScale);

        if (scalingUp)
        {
            currentScale += scaleStep;
            if (currentScale >= maxScale)
                scalingUp = false;
        }
        else
        {
            currentScale -= scaleStep;
            if (currentScale <= minScale)
                scalingUp = true;
        }
    }
}
