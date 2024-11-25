using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformFactory
{
    GameObject CreatePlatform(Vector3 position, string platformType);
    void UpdatePlatformScale(GameObject platformObject);
}
