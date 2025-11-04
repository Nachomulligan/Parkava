using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // METRICS: Enviar evento de zona completada ANTES de cambiar de estado
            var metricsManager = MetricsManager.Instance;
            if (metricsManager != null)
            {
                metricsManager.OnZoneCompleted();
                Debug.Log("[WinTrigger] Level completed! Metrics sent.");
            }

            var gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.GoToWinState();
            }
            else
            {
                Debug.LogError("GameManager instance not found.");
            }
        }
    }
}
