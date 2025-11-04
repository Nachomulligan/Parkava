using System;
using System.Collections;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;

/// <summary>
/// Inicializa Unity Analytics y maneja el consentimiento.
/// </summary>
public class ParkavaAnalyticsManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool autoInitialize = true;
    [SerializeField] private bool debugMode = true;

    private bool _isInitialized = false;

    private async void Start()
    {
        if (autoInitialize)
        {
            await InitializeAnalytics();
        }
    }

    public async System.Threading.Tasks.Task InitializeAnalytics()
    {
        if (_isInitialized)
        {
            Debug.LogWarning("[Analytics] Already initialized");
            return;
        }

        try
        {
            Debug.Log("[Analytics] Initializing Unity Services...");
            await UnityServices.InitializeAsync();

            if (debugMode)
            {
                Debug.Log("[Analytics] Unity Services initialized successfully");
            }

            GiveConsent();
            _isInitialized = true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[Analytics] Failed to initialize: {e.Message}\n{e.StackTrace}");
        }
    }

    private void GiveConsent()
    {
        try
        {
            AnalyticsService.Instance.StartDataCollection();
            
            if (debugMode)
            {
                Debug.Log("[Analytics] Data collection started. Analytics active.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[Analytics] Failed to start data collection: {e.Message}");
        }
    }

    /// <summary>
    /// Para testing: deshabilitar analytics temporalmente
    /// </summary>
    public void StopDataCollection()
    {
        try
        {
            AnalyticsService.Instance.RequestDataDeletion();
            Debug.Log("[Analytics] Data collection stopped");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Analytics] Failed to stop data collection: {e.Message}");
        }
    }

    /// <summary>
    /// Flush manual de eventos (util antes de cerrar el juego)
    /// </summary>
    public void FlushEvents()
    {
        try
        {
            AnalyticsService.Instance.Flush();
            Debug.Log("[Analytics] Events flushed");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Analytics] Failed to flush events: {e.Message}");
        }
    }

    private void OnApplicationQuit()
    {
        // Asegurar que los eventos se envien antes de cerrar
        FlushEvents();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // Flush events cuando la app se pausa (minimiza)
            FlushEvents();
        }
    }
}
