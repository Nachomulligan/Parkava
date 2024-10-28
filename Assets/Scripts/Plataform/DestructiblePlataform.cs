using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlataform : MonoBehaviour
{
    [SerializeField] private float disableDelay = 2f; 

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(disableDelay);
        gameObject.SetActive(false);
    }
}
