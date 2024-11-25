using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleMovingPlatform : Platform
{
    [SerializeField] private float disableDelay = 0.3f;

    private void Update()
    {
        MovePlatform();
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(disableDelay);

        ResetPlatform(); // Desasociar al jugador antes de desactivar
        gameObject.SetActive(false);
    }
}
