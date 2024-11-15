using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformService
{
    GameObject GetPlatform(Vector3 position, string platformType);
    void ReturnPlatform(GameObject platform);
}
