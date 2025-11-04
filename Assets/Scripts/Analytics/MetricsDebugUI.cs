using UnityEngine;
using TMPro;

/// <summary>
/// UI de debug para visualizar m?tricas en tiempo real.
/// </summary>
public class MetricsDebugUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text metricsText;
    [SerializeField] private bool showDebugUI = true;
    [SerializeField] private KeyCode toggleKey = KeyCode.F3;

    private MetricsManager _metricsManager;
    private Canvas _canvas;

    private void Start()
    {
        _metricsManager = MetricsManager.Instance;
        _canvas = GetComponent<Canvas>();

        if (_canvas != null)
        {
            _canvas.enabled = showDebugUI;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            showDebugUI = !showDebugUI;
            if (_canvas != null)
            {
                _canvas.enabled = showDebugUI;
            }
        }

        if (showDebugUI && metricsText != null && _metricsManager != null)
        {
            UpdateMetricsDisplay();
        }
    }

    private void UpdateMetricsDisplay()
    {
        string display = "<b>=== PARKAVA METRICS DEBUG ===</b>\n\n";
        
        display += $"<b>Zone:</b> {_metricsManager.GetCurrentZoneId()}\n";
        display += $"<b>Lives:</b> {_metricsManager.GetLivesRemaining()}/3\n";
        display += $"<b>Attempt #:</b> {_metricsManager.GetCurrentAttemptNumber()}\n";
        display += $"<b>Deaths in Zone:</b> {_metricsManager.GetTotalDeathsInZone()}\n\n";

        display += $"<b>Run Time:</b> {FormatTime(_metricsManager.GetRunTimeSeconds())}\n";
        display += $"<b>Session Time:</b> {FormatTime(_metricsManager.GetSessionTimeSeconds())}\n\n";

        display += $"<b>Last Checkpoint:</b> {_metricsManager.GetLastCheckpointId()}\n\n";

        display += $"<color=yellow>Press {toggleKey} to toggle</color>";

        metricsText.text = display;
    }

    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        int milliseconds = Mathf.FloorToInt((seconds * 1000f) % 1000f);

        return $"{minutes:00}:{secs:00}.{milliseconds:000}";
    }
}
