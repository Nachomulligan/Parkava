using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool
{
    private Queue<GameObject> pool = new Queue<GameObject>();
    private GameObject prefab;
    private Transform parent;

    public ObjectPool(GameObject prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject newObject = GameObject.Instantiate(prefab, parent);
            newObject.SetActive(false);
            pool.Enqueue(newObject);
        }
    }

    public GameObject GetObject(Vector3 position)
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.transform.position = position;
            obj.SetActive(true);
        }
        else
        {
            obj = GameObject.Instantiate(prefab, position, Quaternion.identity, parent);
        }

        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        Platform platform = obj.GetComponent<Platform>();
        platform?.ResetPlatform();

        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
