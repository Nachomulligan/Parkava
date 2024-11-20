using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleMovingPlatform : Platform
{
    [SerializeField] private float disableDelay = 0.3f;

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(disableDelay);
        gameObject.SetActive(false);
    }

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
