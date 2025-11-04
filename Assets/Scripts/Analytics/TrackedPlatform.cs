using UnityEngine;

/// <summary>
/// Componente para trackear cuando el jugador pisa una plataforma.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TrackedPlatform : MonoBehaviour
{
    [Header("Platform Identification")]
    [SerializeField] private string platformId;
    [SerializeField] private PlatformTypeEnum platformType = PlatformTypeEnum.Yellow;

    [Header("Auto-Generate ID")]
    [SerializeField] private bool autoGenerateId = true;

    public enum PlatformTypeEnum
    {
        Yellow,         // Plataforma estable
        Blue,           // Plataforma temporal
        FactoryGenerated // Plataforma generada por f?brica
    }

    private MetricsManager _metricsManager;

    private void Start()
    {
        _metricsManager = MetricsManager.Instance;

        // Auto-generar ID si esta habilitado y no hay uno asignado
        if (autoGenerateId && string.IsNullOrEmpty(platformId))
        {
            platformId = $"{gameObject.scene.name}_{gameObject.name}_{transform.position.GetHashCode()}";
        }

        if (string.IsNullOrEmpty(platformId))
        {
            Debug.LogWarning($"[TrackedPlatform] Platform has no ID assigned: {gameObject.name}");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar que sea el jugador
        if (!collision.gameObject.CompareTag("Player")) return;

        var playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement == null) return;

        if (playerMovement.grounded)
        {
            OnPlayerSteppedOn();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        var playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null && playerMovement.grounded)
        {
            // Solo enviar si no esta ya en el suelo para evitar spam
            return; 
        }
    }

    private void OnPlayerSteppedOn()
    {
        if (_metricsManager == null) return;

        string typeString = platformType switch
        {
            PlatformTypeEnum.Yellow => "yellow",
            PlatformTypeEnum.Blue => "blue",
            PlatformTypeEnum.FactoryGenerated => "factory_generated",
            _ => "unknown"
        };

        _metricsManager.OnPlatformSteppedOn(platformId, typeString);
        Debug.Log($"[TrackedPlatform] Player stepped on {platformId} ({typeString})");
    }

    /// <summary>
    /// Resetear el tracking (ya no es necesario, pero se mantiene por compatibilidad)
    /// </summary>
    public void ResetTracking()
    {

    }

    private void OnValidate()
    {
        if (autoGenerateId && string.IsNullOrEmpty(platformId))
        {
            platformId = $"platform_{gameObject.name}";
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = platformType switch
        {
            PlatformTypeEnum.Yellow => Color.yellow,
            PlatformTypeEnum.Blue => Color.cyan,
            PlatformTypeEnum.FactoryGenerated => Color.magenta,
            _ => Color.white
        };

        Gizmos.DrawWireCube(transform.position, GetComponent<Collider>().bounds.size);
    }
}
