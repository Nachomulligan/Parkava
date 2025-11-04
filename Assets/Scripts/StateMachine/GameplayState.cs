using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : IState
{
    public string Id { get; private set; } = "Gameplay";
    public Dictionary<string, IState> Outputs { get; private set; }

    public GameplayState()
    {
        Outputs = new Dictionary<string, IState>();
    }

    public void Enter()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Entered Gameplay State");

        // Timer Service
        var timerService = ServiceLocator.Instance.GetService(nameof(TimerService)) as TimerService;
        if (timerService != null)
        {
            timerService.StartRun();
        }
        else
        {
            Debug.LogWarning("TimerService not found in ServiceLocator.");
        }

        // METRICS: Iniciar run y resumir gameplay
        var metricsManager = MetricsManager.Instance;
        if (metricsManager != null)
        {
            // IMPORTANTE: Iniciar el run con la zona y cantidad de checkpoints
            // TODO: Si tienes múltiples zonas, detectar cuál es la zona actual
            // Por ahora, asumimos que siempre empiezas en zone_1
            metricsManager.OnRunStarted("zone_1", 1); // 1 checkpoint por zona
            
            metricsManager.ResumeGameplay();
            Debug.Log("[GameplayState] Metrics run started for zone_1");
        }
        else
        {
            Debug.LogWarning("[GameplayState] MetricsManager not found!");
        }
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        Debug.Log("Exiting Gameplay State");

        // Timer Service
        var timerService = ServiceLocator.Instance.GetService(nameof(TimerService)) as TimerService;
        if (timerService != null)
        {
            timerService.PauseRun();
        }

        // METRICS: Pausar gameplay (timers pausados)
        var metricsManager = MetricsManager.Instance;
        if (metricsManager != null)
        {
            metricsManager.PauseGameplay();
        }
    }
}
