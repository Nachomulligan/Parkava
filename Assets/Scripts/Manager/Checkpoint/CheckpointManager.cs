using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Checkpoint lastCheckpoint;

    [Header("Respawn Settings")]
    [Tooltip("Additional safety offset applied to all respawns")]
    [SerializeField] private Vector3 globalSpawnOffset = Vector3.zero;

    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(CheckpointManager), this);
    }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        lastCheckpoint = checkpoint;
        Debug.Log($"Checkpoint updated: {checkpoint.transform.position}");
    }

    public void Respawn(GameObject player)
    {
        if (lastCheckpoint != null)
        {
            Vector3 respawnPosition = lastCheckpoint.SpawnPosition + globalSpawnOffset;
            player.transform.position = respawnPosition;
            player.SetActive(true);

            var character = ServiceLocator.Instance.GetService(nameof(Character)) as Character;
            if (character != null && character.health != null)
            {
                character.health.Heal(character.health.GetMaxHealth());
                Debug.Log($"Player respawned at: {respawnPosition}");
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

    public Vector3? GetLastCheckpointPosition()
    {
        return lastCheckpoint != null ? lastCheckpoint.SpawnPosition : null;
    }
}