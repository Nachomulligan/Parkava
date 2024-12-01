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
            var health = player.GetComponent<Character>().health;
            health.Heal(health.GetMaxHealth());
            Debug.Log("Player respawned at: " + lastCheckpoint.position);
        }
        else
        {
            Debug.LogWarning("No checkpoint set. Cannot respawn.");
        }
    }
}
