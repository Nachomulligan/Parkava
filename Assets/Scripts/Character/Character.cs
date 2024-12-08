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

    [Header("Health Settings")]
    public IHealth health;
    [SerializeField] private int initialHealth = 1;
    

    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(Character), this);
    }
    void Start()
    {
        health = new Health(initialHealth);
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

    public void SetHP(int value)
    {
        health = new Health(value);
    }

    public void SetShield(int shieldAmount)
    {
        if (health is ShieldDecorator shieldDecorator)
        {
            shieldDecorator.SetShield(shieldAmount);
        }
        else
        {
            health = new ShieldDecorator(health, shieldAmount);
        }

        Debug.Log($"Shield set to {shieldAmount}");
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
                var checkpointManager = ServiceLocator.Instance.GetService(nameof(CheckpointManager)) as CheckpointManager;
                if (checkpointManager != null)
                {
                    checkpointManager.Respawn(gameObject);
                }
                else
                {
                    Debug.LogError("CheckpointManager not found in the ServiceLocator.");
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
    }
}
