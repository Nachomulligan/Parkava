using HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactionLayer;
    private Collider[] interactables = new Collider[5];
    public IHealth health;
    void Start()
    {
        health = new Health(1);
        health.OnDeath += Die;
        var platformService = ServiceLocator.Instance.GetService(nameof(DestructiblePlatformService)) as DestructiblePlatformService;
        if (platformService != null)
        {
            health.OnDeath += platformService.ReactivateAllPlatforms;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Debug.Log("Tried interacting");

        int elements = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionRadius,
            interactables, interactionLayer);
        if (elements == 0)
        {
            Debug.Log("No interactables found");
            return;
        }

        for (int i = 0; i < elements; i++)
        {
            var interactable = interactables[i];

            var interactableComponent = interactable.GetComponent<IInteractable>();

            if (interactableComponent != null)
            {
                interactableComponent.Interact();
                return;
            }
        }
    }

    private void Die()
    {
        Debug.Log("Character has died.");
        gameObject.SetActive(false);

        var lifeService = ServiceLocator.Instance.GetService(nameof(LifeService)) as LifeService;

        if (lifeService != null)
        {
            lifeService.ReduceLife();

            if (lifeService.GetCurrentLives() > 0)
            {
                var platformService = ServiceLocator.Instance.GetService(nameof(DestructiblePlatformService)) as DestructiblePlatformService;
                if (platformService != null)
                {
                    Debug.Log("Reactivating platforms...");
                    platformService.ReactivateAllPlatforms();
                }
                var checkpointManager = FindObjectOfType<CheckpointManager>();
                if (checkpointManager != null)
                {
                    checkpointManager.Respawn(gameObject);
                }
                else
                {
                    Debug.LogError("CheckpointManager not found in the scene.");
                }
            }
            else
            {
                Debug.Log("Game Over: Permadeath triggered.");
                lifeService.OnPermadeath += HandlePermadeath;
            }
        }
        else
        {
            Debug.LogError("LifeService not found in the scene.");
        }
    }

    private void HandlePermadeath()
    {
        Debug.Log("Game Over: Permadeath triggered.");
        var platformService = ServiceLocator.Instance.GetService(nameof(DestructiblePlatformService)) as DestructiblePlatformService;
        if (platformService != null)
        {
            health.OnDeath -= platformService.ReactivateAllPlatforms;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);

    }

}
