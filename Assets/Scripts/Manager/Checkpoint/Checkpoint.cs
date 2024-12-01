using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var checkpointManager = ServiceLocator.Instance.GetService(nameof(CheckpointManager)) as CheckpointManager;
            if (checkpointManager != null)
            {
                checkpointManager.SetCheckpoint(transform);
                Debug.Log("Checkpoint reached at: " + transform.position);
            }
            else
            {
                Debug.LogError("CheckpointManager not found in the scene.");
            }
        }
    }
}
