using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlatform : MonoBehaviour
{
    [SerializeField] private float disableDelay = 2f;

    private DestructiblePlatformService platformService;

    private void Awake()
    {
        platformService = ServiceLocator.Instance.GetService(nameof(DestructiblePlatformService)) as DestructiblePlatformService;

        if (platformService != null)
        {
            platformService.RegisterPlatform(this);
        }
    }

    private void OnDestroy()
    {
        if (platformService != null)
        {
            platformService.UnregisterPlatform(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(disableDelay);
        gameObject.SetActive(false);
    }

    public void Reactivate()
    {
        gameObject.SetActive(true);
    }
}
