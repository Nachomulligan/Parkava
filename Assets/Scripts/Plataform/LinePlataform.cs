using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePlatform : Platform
{
    private void Update()
    {
        MovePlatform();
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log($"Trigger detectado con: {other.name}");
        if (other.CompareTag("Player")) 
        {
            other.transform.SetParent(transform);
            Debug.Log("Personaje se unió a la plataforma lineal");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == transform) 
        {
            other.transform.SetParent(null);
            Debug.Log("Personaje se separó de la plataforma lineal");
        }
    }
}