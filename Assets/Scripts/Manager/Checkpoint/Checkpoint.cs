using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Offset from the checkpoint position for spawning (useful to spawn on ground)")]
    [SerializeField] private Vector3 spawnOffset = Vector3.zero;

    [Header("Visual Feedback")]
    [SerializeField] private bool drawGizmo = true;
    [SerializeField] private Color gizmoColor = Color.green;
    public Vector3 SpawnPosition => transform.position + spawnOffset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var checkpointManager = ServiceLocator.Instance.GetService(nameof(CheckpointManager)) as CheckpointManager;
            if (checkpointManager != null)
            {
                checkpointManager.SetCheckpoint(this);
                Debug.Log($"Checkpoint reached at: {transform.position}, Spawn position: {SpawnPosition}");
            }
            else
            {
                Debug.LogError("CheckpointManager not found in the scene.");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, 0.5f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(SpawnPosition, 0.3f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, SpawnPosition);
        }
    }
}