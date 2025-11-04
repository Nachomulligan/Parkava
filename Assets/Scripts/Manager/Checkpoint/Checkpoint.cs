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

    [Header("Metrics Tracking")]
    [Tooltip("ID del checkpoint para m?tricas (ej: zone1_cp1)")]
    [SerializeField] private string checkpointId;
    [SerializeField] private bool autoGenerateId = false;

    public Vector3 SpawnPosition => transform.position + spawnOffset;

    private MetricsManager _metricsManager;
    private bool _hasBeenReached = false;

    private void Start()
    {
        _metricsManager = MetricsManager.Instance;

        // Auto-generar ID si esta habilitado
        if (autoGenerateId && string.IsNullOrEmpty(checkpointId))
        {
            checkpointId = $"{gameObject.scene.name}_cp_{transform.position.GetHashCode()}";
        }

        if (string.IsNullOrEmpty(checkpointId))
        {
            Debug.LogWarning($"[Checkpoint] Checkpoint has no ID assigned: {gameObject.name}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // PRIMERO: Actualizar CheckpointManager (respawn)
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

            // SEGUNDO: Enviar evento de metricas (solo una vez por run)
            if (!_hasBeenReached && _metricsManager != null && !string.IsNullOrEmpty(checkpointId))
            {
                _hasBeenReached = true;
                _metricsManager.OnCheckpointReached(checkpointId);
                Debug.Log($"[Checkpoint] Metrics sent for checkpoint: {checkpointId}");
            }
        }
    }

    /// <summary>
    /// Resetear el checkpoint (llamar cuando empieza un nuevo run)
    /// </summary>
    public void ResetCheckpoint()
    {
        _hasBeenReached = false;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = _hasBeenReached ? Color.gray : gizmoColor;
            Gizmos.DrawWireSphere(transform.position, 0.5f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(SpawnPosition, 0.3f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, SpawnPosition);
        }
    }
}