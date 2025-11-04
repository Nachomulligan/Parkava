using UnityEngine;

/// <summary>
/// Componente para checkpoints que trackea cuando el jugador los alcanza.
/// Se usa JUNTO con el componente Checkpoint.cs existente.
/// Checkpoint.cs maneja el respawn, TrackedCheckpoint maneja las m?tricas.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TrackedCheckpoint : MonoBehaviour
{
    [Header("Checkpoint Identification")]
    [Tooltip("ID del checkpoint (ej: zone1_cp1, zone2_cp1, zone3_cp1, zone4_cp1)")]
    [SerializeField] private string checkpointId;
    
    [Header("Auto-Generate ID")]
    [Tooltip("Si est? desactivado, usa el checkpointId manual de arriba")]
    [SerializeField] private bool autoGenerateId = false;

    [Header("Visual Feedback")]
    [Tooltip("GameObject que se activa al alcanzar el checkpoint (opcional)")]
    [SerializeField] private GameObject activatedVisual;
    [SerializeField] private Color gizmoColor = Color.green;

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
            Debug.LogWarning($"[TrackedCheckpoint] Checkpoint has no ID assigned: {gameObject.name}");
        }

        if (activatedVisual != null)
        {
            activatedVisual.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!_hasBeenReached)
        {
            OnCheckpointReached();
        }
    }

    private void OnCheckpointReached()
    {
        if (_hasBeenReached || _metricsManager == null) return;

        _hasBeenReached = true;

        _metricsManager.OnCheckpointReached(checkpointId);

        if (activatedVisual != null)
        {
            activatedVisual.SetActive(true);
        }

        Debug.Log($"[TrackedCheckpoint] Checkpoint reached: {checkpointId}");

        PlayCheckpointEffects();
    }

    private void PlayCheckpointEffects()
    {
        var audioService = ServiceLocator.Instance.GetService(nameof(AudioService)) as AudioService;
        if (audioService != null)
        {
            audioService.PlaySFX("Checkpoint");
        }

    }

    /// <summary>
    /// Resetear el checkpoint (llamar cuando empieza un nuevo run)
    /// </summary>
    public void ResetCheckpoint()
    {
        _hasBeenReached = false;
        
        if (activatedVisual != null)
        {
            activatedVisual.SetActive(false);
        }
    }

    private void OnValidate()
    {
        if (autoGenerateId && string.IsNullOrEmpty(checkpointId))
        {
            checkpointId = $"cp_{gameObject.name}";
        }

        var col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
        }
    }

    private void OnDrawGizmos()
    {
        var hasCheckpointComponent = GetComponent<Checkpoint>() != null;
        if (hasCheckpointComponent) return;

        Gizmos.color = _hasBeenReached ? Color.gray : gizmoColor;
        
        var col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.DrawWireCube(transform.position, col.bounds.size);
            
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f);
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 2f, 0.3f);
        }
    }
}
