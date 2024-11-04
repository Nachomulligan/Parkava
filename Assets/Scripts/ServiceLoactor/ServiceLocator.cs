using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    Dictionary<string, MonoBehaviour> servicesByName = new();

    private static ServiceLocator _instance;
    public static ServiceLocator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ServiceLocator>();
            }

            if (_instance == null)
            {
                var newGO = new GameObject();
                _instance = newGO.AddComponent<ServiceLocator>();
            }

            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }
    public MonoBehaviour GetService(string serviceName)
    {
        // retrieves a service by name
        servicesByName.TryGetValue(serviceName, out var service);
        return service;
    }

    public void SetService(string serviceName, MonoBehaviour value)
    {
        // registers a service with a name
        if (!servicesByName.ContainsKey(serviceName))
        {
            servicesByName.Add(serviceName, value);
        }
    }
    public bool IsInitialized()
    {
        return _instance != null;
    }
}
