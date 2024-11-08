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
}
