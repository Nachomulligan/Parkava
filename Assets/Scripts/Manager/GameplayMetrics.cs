using Unity.Services.Analytics;
using UnityEngine;


public class GameplayMetrics : MonoBehaviour
{
    private PlayerDataManager _playerDataManager;
    private string _currentPlayerId;

    private void Start()
    {
        _playerDataManager = PlayerDataManager.Instance;

        if (_playerDataManager != null && _playerDataManager.IsPlayerIdSet())
        {
            _currentPlayerId = _playerDataManager.GetPlayerId();
            Debug.Log($"[Gameplay] Player ID loaded: {_currentPlayerId}");

            var runEvent = new RunStartedEvent();
            runEvent.PlayerId = _currentPlayerId;
            runEvent.CurrentAttemptNumber = _playerDataManager.RunCount;
            AnalyticsService.Instance.RecordEvent(runEvent);
            InitializeMetrics(_currentPlayerId);
        }
        else
        {
            Debug.LogError("[Gameplay] Player ID not set! Returning to Main Menu...");
            ReturnToMainMenu();
        }
    }

    private void InitializeMetrics(string playerId)
    {
        // EJEMPLO: Aqu? es donde configurar?as Unity Analytics o tus m?tricas
        // Unity.Analytics.Analytics.SetUserId(playerId);
        
        Debug.Log($"Metrics initialized for Player: {playerId}");
        
        // Ejemplo de evento de m?trica
        SendMetricEvent("GameplayStarted", playerId);
    }

    private void SendMetricEvent(string eventName, string playerId)
    {
        // EJEMPLO: Aqu? enviar?as eventos a tu sistema de m?tricas
        // Unity.Analytics.Analytics.CustomEvent(eventName, new Dictionary<string, object>
        // {
        //     { "player_id", playerId },
        //     { "timestamp", System.DateTime.UtcNow.ToString() }
        // });
        
        Debug.Log($"Metric Event Sent: {eventName} for Player: {playerId}");
    }

    /// <summary>
    /// Obtiene el Player ID actual para usar en otros sistemas
    /// </summary>
    public string GetCurrentPlayerId()
    {
        return _currentPlayerId;
    }

    private void ReturnToMainMenu()
    {
        var sceneController = ServiceLocator.Instance.GetService(nameof(SceneController)) as SceneController;
        if (sceneController != null)
        {
            sceneController.LoadSceneByName("MainMenu");
        }
    }

    // Ejemplo de c?mo enviar eventos durante el gameplay
    public void OnPlayerDied()
    {
        if (!string.IsNullOrEmpty(_currentPlayerId))
        {
            SendMetricEvent("PlayerDeath", _currentPlayerId);
        }
    }

    public void OnLevelCompleted(int level)
    {
        if (!string.IsNullOrEmpty(_currentPlayerId))
        {
            SendMetricEvent($"LevelCompleted_{level}", _currentPlayerId);
        }
    }
}
