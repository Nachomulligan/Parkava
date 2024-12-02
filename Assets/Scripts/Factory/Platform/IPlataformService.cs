using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformService
{
    GameObject GetPlatform(Vector3 position);
    void Initialize(List<GameObject> prefabs, float minScale, float maxScale, float scaleStep);
    void ReturnPlatform(GameObject platform);
}
