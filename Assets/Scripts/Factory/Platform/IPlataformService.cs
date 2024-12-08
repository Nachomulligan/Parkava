using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformService
{
    GameObject GetPlatform(Vector3 position);
    void Initialize(Dictionary<string, GameObject> prefabs, float minScale, float maxScale, float scaleStep, int initialPoolSize);
    void ReturnPlatform(GameObject platform);
}
