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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);

    }

}
