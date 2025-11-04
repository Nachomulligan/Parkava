using UnityEngine;

/// <summary>
/// Gestor de zona que maneja el inicio de runs.
/// Hay 4 ZoneManagers en el nivel (uno por zona).
/// El WinTrigger unico maneja la victoria al final.
/// </summary>
public class ZoneManager : MonoBehaviour
{
    [Header("Zone Configuration")]
    [Tooltip("ID de la zona: zone_1, zone_2, zone_3, zone_4")]
    [SerializeField] private string zoneId = "zone_1";
    
    [Header("Checkpoints")]
    [Tooltip("Total de checkpoints en ESTA zona (siempre 1)")]
    [SerializeField] private int totalCheckpoints = 1;

    [Header("Start Point")]
    [Tooltip("Punto de inicio del jugador en esta zona")]
    [SerializeField] private Transform startPoint;

    private MetricsManager _metricsManager;
    private bool _hasStartedRun = false;

    private void Start()
    {
        _metricsManager = MetricsManager.Instance;
    }

    /// <summary>
    /// Llamar cuando el jugador entra al nivel y presiona Play.
    /// Solo se llama desde la Zona 1 (inicio del nivel).
    /// </summary>
    public void StartRun()
    {
        if (_metricsManager == null) return;

        _hasStartedRun = true;

        // METRICS: Iniciar run
        _metricsManager.OnRunStarted(zoneId, totalCheckpoints);

        // Resetear checkpoints de TODO el nivel
        ResetAllCheckpoints();

        // Resetear plataformas tracked de TODO el nivel
        ResetAllTrackedPlatforms();

        Debug.Log($"[ZoneManager] Run started in {zoneId}");
    }

    /// <summary>
    /// Llamar cuando el jugador sale del nivel sin completarlo
    /// </summary>
    public void ExitLevel()
    {
        if (_metricsManager != null && _hasStartedRun)
        {
            _metricsManager.OnRunExited();
        }

        _hasStartedRun = false;
    }

    /// <summary>
    /// Obtener el zone ID actual (para encontrar el ZoneManager correcto)
    /// </summary>
    public string GetCurrentZoneId()
    {
        return zoneId;
    }

    private void ResetAllCheckpoints()
    {
        var checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.ResetCheckpoint();
        }
        
        var trackedCheckpoints = FindObjectsOfType<TrackedCheckpoint>();
        foreach (var trackedCheckpoint in trackedCheckpoints)
        {
            trackedCheckpoint.ResetCheckpoint();
        }
    }

    private void ResetAllTrackedPlatforms()
    {
        var platforms = FindObjectsOfType<TrackedPlatform>();
        foreach (var platform in platforms)
        {
            platform.ResetTracking();
        }
    }

    private void OnValidate()
    {
        if (startPoint == null)
        {
            var startObj = GameObject.Find("StartPoint");
            if (startObj != null)
            {
                startPoint = startObj.transform;
            }
        }

        if (totalCheckpoints != 1)
        {
            Debug.LogWarning($"[ZoneManager] Total checkpoints debe ser 1 (cada zona tiene 1 checkpoint). Corrigiendo...");
            totalCheckpoints = 1;
        }
    }

    private void OnDrawGizmos()
    {
        if (startPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPoint.position, 1f);
            Gizmos.DrawLine(startPoint.position, startPoint.position + Vector3.up * 3f);
        }

        #if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 5f, $"Zone Manager: {zoneId}");
        #endif
    }
}
