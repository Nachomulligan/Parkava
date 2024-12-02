using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Transform lastCheckpoint;

    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(CheckpointManager), this);
    }
    public void SetCheckpoint(Transform checkpoint)
    {
        lastCheckpoint = checkpoint;
        Debug.Log("Checkpoint updated: " + checkpoint.position);
    }

    public void Respawn(GameObject player)
    {
        if (lastCheckpoint != null)
        {
            player.transform.position = lastCheckpoint.position;
            player.SetActive(true);
            var character = ServiceLocator.Instance.GetService(nameof(Character)) as Character;
            if (character != null && character.health != null)
            {
                character.health.Heal(character.health.GetMaxHealth());
                Debug.Log("Player respawned at: " + lastCheckpoint.position);
            }
            else
            {
                Debug.LogError("Character or its health component not found in ServiceLocator.");
            }
        }
        else
        {
            Debug.LogWarning("No checkpoint set. Cannot respawn.");
        }
    }
}
