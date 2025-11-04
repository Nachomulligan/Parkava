using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

/// <summary>
/// Gestor centralizado de metricas y analytics del juego.
/// Maneja contadores, timers y env?o de eventos segun el Metrics Plan.
/// </summary>
public class MetricsManager : MonoBehaviour
{
    private static MetricsManager _instance;
    
    // IDs Persistentes
    private string _playerId;
    private string _currentSessionId;
    
    // Estado Actual 
    private string _currentZoneId;
    
    // Contadores por Zona
    private int _totalAttemptsInZone;
    private int _currentAttemptNumber;
    private int _totalDeathsInZone;
    
    // Timers (en segundos)
    private float _runTimeSeconds;
    private float _checkpointTimeSeconds;
    private float _sessionTimeInZoneSeconds;
    
    //  Estado de Checkpoints
    private string _lastCheckpointId = "start";
    private string _previousCheckpointId = "start";
    private int _totalCheckpointsInZone = 0;
    private int _livesRemaining = 3;
    
    // Tracking de Sesion
    private List<string> _zonesAttemptedInSession = new List<string>();
    private int _totalAttemptsInSession;
    private string _furthestCheckpointInSession = "start";
    private DateTime _sessionStartTime;
    
    // Estados de Control
    private bool _isGameplayActive = false;
    private bool _isSessionActive = false;
    private float _sessionStartRealTime;
    
    // Checkpoints visitados en run actual 
    private HashSet<string> _checkpointsVisitedThisRun = new HashSet<string>();

    public static MetricsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MetricsManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("MetricsManager");
                    _instance = go.AddComponent<MetricsManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        ServiceLocator.Instance.SetService(nameof(MetricsManager), this);
        
        InitializeSession();
    }

    private void InitializeSession()
    {
        // Obtener o generar Player ID desde PlayerDataManager
        var playerDataManager = PlayerDataManager.Instance;
        if (playerDataManager != null && playerDataManager.IsPlayerIdSet())
        {
            _playerId = playerDataManager.GetPlayerId();
        }
        else
        {
            _playerId = Guid.NewGuid().ToString();
            Debug.LogWarning($"Player ID not set in PlayerDataManager, generated temporary ID: {_playerId}");
        }

        // Generar nuevo Session ID
        _currentSessionId = Guid.NewGuid().ToString();
        _sessionStartTime = DateTime.UtcNow;
        _sessionStartRealTime = Time.realtimeSinceStartup;
        _isSessionActive = true;

        Debug.Log($"[MetricsManager] Session initialized - PlayerID: {_playerId}, SessionID: {_currentSessionId}");
    }

    private void Update()
    {
        if (!_isSessionActive) return;

        // Incrementar session time siempre que el juego esta abierto
        _sessionTimeInZoneSeconds += Time.unscaledDeltaTime;

        // Incrementar run time y checkpoint time solo durante gameplay activo
        if (_isGameplayActive)
        {
            _runTimeSeconds += Time.deltaTime;
            _checkpointTimeSeconds += Time.deltaTime;
        }
    }

    // METODOS PUBLICOS PARA EVENTOS

    /// <summary>
    /// Llamar cuando el jugador inicia un nuevo run
    /// </summary>
    public void OnRunStarted(string zoneId, int totalCheckpoints)
    {
        _currentZoneId = zoneId;
        _totalCheckpointsInZone = totalCheckpoints;
        _currentAttemptNumber++;
        _totalAttemptsInZone++;
        _totalAttemptsInSession++;
        _livesRemaining = 3;
        _runTimeSeconds = 0f;
        _checkpointTimeSeconds = 0f;
        _lastCheckpointId = "start";
        _previousCheckpointId = "start";
        _checkpointsVisitedThisRun.Clear();
        _isGameplayActive = true;

        if (!_zonesAttemptedInSession.Contains(zoneId))
        {
            _zonesAttemptedInSession.Add(zoneId);
        }

        RunStartedEvent evt = new RunStartedEvent
        {
            PlayerId = _playerId,
            ZoneId = zoneId,
            CurrentAttemptNumber = _currentAttemptNumber,
            SessionId = _currentSessionId
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log($"[Metrics] Run Started - Zone: {zoneId}, Attempt: {_currentAttemptNumber}");
    }

    /// <summary>
    /// Llamar cuando el jugador pisa una plataforma
    /// </summary>
    public void OnPlatformSteppedOn(string platformId, string platformType)
    {
        // VALIDACION: Asegurar que no haya valores null antes de enviar
        if (string.IsNullOrEmpty(_currentZoneId))
        {
            Debug.LogWarning("[Metrics] Zone ID is null. Did you call StartRun()? Skipping platform event.");
            return;
        }

        if (string.IsNullOrEmpty(platformId))
        {
            Debug.LogWarning("[Metrics] Platform ID is null, using fallback");
            platformId = "unknown_platform";
        }

        if (string.IsNullOrEmpty(platformType))
        {
            Debug.LogWarning("[Metrics] Platform type is null, using fallback");
            platformType = "unknown";
        }

        PlatformSteppedOnEvent evt = new PlatformSteppedOnEvent
        {
            PlayerId = _playerId ?? "unknown",
            PlatformId = platformId,
            PlatformType = platformType,
            CurrentAttemptNumber = _currentAttemptNumber,
            LivesRemaining = _livesRemaining,
            RunTimeSeconds = Mathf.FloorToInt(_runTimeSeconds),
            CheckpointTimeSeconds = Mathf.FloorToInt(_checkpointTimeSeconds),
            SessionTimeSeconds = Mathf.FloorToInt(_sessionTimeInZoneSeconds),
            ZoneId = _currentZoneId
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log($"[Metrics] Platform Stepped On - ID: {platformId}, Type: {platformType}, Zone: {_currentZoneId}");
    }


    /// <summary>
    /// Llamar cuando el jugador muere
    /// </summary>
    public void OnPlayerDied(string deathCause, Vector3 deathPosition)
    {
        _totalDeathsInZone++;
        _livesRemaining--;
        
        PlayerDiedEvent evt = new PlayerDiedEvent
        {
            PlayerId = _playerId,
            DeathCause = deathCause,
            DeathPositionX = deathPosition.x,
            DeathPositionY = deathPosition.y,
            DeathPositionZ = deathPosition.z,
            ZoneId = _currentZoneId,
            CurrentAttemptNumber = _currentAttemptNumber,
            RunTimeSeconds = Mathf.FloorToInt(_runTimeSeconds),
            CheckpointTimeSeconds = Mathf.FloorToInt(_checkpointTimeSeconds),
            SessionTimeSeconds = Mathf.FloorToInt(_sessionTimeInZoneSeconds),
            LastCheckpointId = _lastCheckpointId
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log($"[Metrics] Player Died - Cause: {deathCause}, Lives Remaining: {_livesRemaining}");

        // Resetear checkpoint time (vuelve desde ?ltimo checkpoint)
        _checkpointTimeSeconds = 0f;

        // Si se quedo sin vidas, hacer game over
        if (_livesRemaining <= 0)
        {
            OnGameOver();
        }
    }

    /// <summary>
    /// Llamar cuando el jugador alcanza un checkpoint
    /// </summary>
    public void OnCheckpointReached(string checkpointId)
    {
        // Verificar que no se haya tocado ya en este run
        if (_checkpointsVisitedThisRun.Contains(checkpointId))
        {
            return;
        }

        _checkpointsVisitedThisRun.Add(checkpointId);
        _previousCheckpointId = _lastCheckpointId;
        
        CheckpointReachedEvent evt = new CheckpointReachedEvent
        {
            PlayerId = _playerId,
            CheckpointId = checkpointId,
            ZoneId = _currentZoneId,
            CurrentAttemptNumber = _currentAttemptNumber,
            LivesRemaining = _livesRemaining,
            TotalAttemptsInZone = _totalAttemptsInZone,
            CheckpointTimeSeconds = Mathf.FloorToInt(_checkpointTimeSeconds),
            SessionTimeSeconds = Mathf.FloorToInt(_sessionTimeInZoneSeconds),
            PreviousCheckpointId = _previousCheckpointId
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log($"[Metrics] Checkpoint Reached - ID: {checkpointId}");

        // Actualizar checkpoint actual y resetear timer
        _lastCheckpointId = checkpointId;
        _checkpointTimeSeconds = 0f;

        // Actualizar el checkpoint mas lejano de la sesi?n
        _furthestCheckpointInSession = checkpointId;
    }

    /// <summary>
    /// Llamar cuando el jugador completa una zona exitosamente
    /// </summary>
    public void OnZoneCompleted()
    {
        ZoneWinReachedEvent evt = new ZoneWinReachedEvent
        {
            PlayerId = _playerId,
            ZoneId = _currentZoneId,
            TotalAttemptsInZone = _totalAttemptsInZone,
            TotalDeathsInZone = _totalDeathsInZone,
            WinCompletionTimeSeconds = _runTimeSeconds,
            TotalCheckpointsInZone = _totalCheckpointsInZone,
            SessionId = _currentSessionId,
            SessionTimeInZoneSeconds = Mathf.FloorToInt(_sessionTimeInZoneSeconds)
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log($"[Metrics] Zone Completed - Zone: {_currentZoneId}, Time: {_runTimeSeconds}s");

        ResetZoneCounters();
    }

    /// <summary>
    /// Llamar cuando el jugador sale del run sin completarlo
    /// </summary>
    public void OnRunExited()
    {
        RunExitedEvent evt = new RunExitedEvent
        {
            PlayerId = _playerId,
            ZoneId = _currentZoneId,
            CurrentAttemptNumber = _currentAttemptNumber,
            RunTimeSeconds = Mathf.FloorToInt(_runTimeSeconds),
            FurthestCheckpointReached = _lastCheckpointId
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log($"[Metrics] Run Exited - Zone: {_currentZoneId}");

        _isGameplayActive = false;
    }

    /// <summary>
    /// Llamar cuando se cambia de zona (sin completar la actual)
    /// </summary>
    public void OnZoneChanged(string newZoneId)
    {
        if (!string.IsNullOrEmpty(_currentZoneId) && _currentZoneId != newZoneId)
        {
            OnRunExited();
        }

        ResetZoneCounters();
        _currentZoneId = newZoneId;
    }

    /// <summary>
    /// Llamar cuando el jugador se queda sin vidas (Game Over)
    /// </summary>
    private void OnGameOver()
    {
        Debug.Log($"[Metrics] Game Over - Resetting run counters");

        // Resetear contadores del run
        _runTimeSeconds = 0f;
        _checkpointTimeSeconds = 0f;
        _livesRemaining = 3;
        _lastCheckpointId = "start";
        _checkpointsVisitedThisRun.Clear();
        _isGameplayActive = false;

        // NO resetear: TotalAttemptsInZone, TotalDeathsInZone, SessionTimeInZoneSeconds
    }

    /// <summary>
    /// Resetear todos los contadores de zona
    /// </summary>
    private void ResetZoneCounters()
    {
        _totalAttemptsInZone = 0;
        _currentAttemptNumber = 0;
        _totalDeathsInZone = 0;
        _sessionTimeInZoneSeconds = 0f;
        _runTimeSeconds = 0f;
        _checkpointTimeSeconds = 0f;
        _lastCheckpointId = "start";
        _previousCheckpointId = "start";
        _livesRemaining = 3;
        _checkpointsVisitedThisRun.Clear();
    }

    /// <summary>
    /// Pausar gameplay (no cuenta run time ni checkpoint time)
    /// </summary>
    public void PauseGameplay()
    {
        _isGameplayActive = false;
    }

    public void ResumeGameplay()
    {
        _isGameplayActive = true;
    }

    /// <summary>
    /// Finalizar sesion (llamar al cerrar el juego)
    /// </summary>
    public void EndSession()
    {
        if (!_isSessionActive) return;

        float sessionDuration = (Time.realtimeSinceStartup - _sessionStartRealTime) / 60f;

        SessionEndedEvent evt = new SessionEndedEvent
        {
            PlayerId = _playerId,
            SessionId = _currentSessionId,
            SessionDurationMinutes = sessionDuration,
            TotalAttemptsInSession = _totalAttemptsInSession,
            ZonesAttemptedList = string.Join(",", _zonesAttemptedInSession),
            FurthestCheckpointReached = _furthestCheckpointInSession
        };

        AnalyticsService.Instance.RecordEvent(evt);
        Debug.Log($"[Metrics] Session Ended - Duration: {sessionDuration:F2} minutes");

        _isSessionActive = false;
    }

    private void OnApplicationQuit()
    {
        EndSession();
    }

    
    public int GetLivesRemaining() => _livesRemaining;
    public int GetCurrentAttemptNumber() => _currentAttemptNumber;
    public int GetTotalDeathsInZone() => _totalDeathsInZone;
    public float GetRunTimeSeconds() => _runTimeSeconds;
    public float GetSessionTimeSeconds() => _sessionTimeInZoneSeconds;
    public string GetCurrentZoneId() => _currentZoneId;
    public string GetLastCheckpointId() => _lastCheckpointId;
}
