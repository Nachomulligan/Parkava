using UnityEngine;

/// <summary>
/// Version ALTERNATIVA con cooldown para evitar spam de eventos.
/// Envia un evento cada X segundos por plataforma.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TrackedPlatformWithCooldown : MonoBehaviour
{
    [Header("Platform Identification")]
    [SerializeField] private string platformId;
    [SerializeField] private PlatformTypeEnum platformType = PlatformTypeEnum.Yellow;

    [Header("Auto-Generate ID")]
    [SerializeField] private bool autoGenerateId = true;

    [Header("Cooldown Settings")]
    [Tooltip("Tiempo m?nimo entre eventos de la misma plataforma (segundos)")]
    [SerializeField] private float eventCooldown = 0.5f;

    public enum PlatformTypeEnum
    {
        Yellow,
        Blue,
        FactoryGenerated
    }

    private MetricsManager _metricsManager;
    private float _lastEventTime = -999f;

    private void Start()
    {
        _metricsManager = MetricsManager.Instance;

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
        if (!collision.gameObject.CompareTag("Player")) return;

        var playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement == null) return;

        if (playerMovement.grounded)
        {
            OnPlayerSteppedOn();
        }
    }

    private void OnPlayerSteppedOn()
    {
        if (_metricsManager == null) return;

        // ? Cooldown: Solo enviar si ha pasado suficiente tiempo
        if (Time.time - _lastEventTime < eventCooldown)
        {
            return; // Ignorar si el cooldown no ha expirado
        }

        _lastEventTime = Time.time;

        string typeString = platformType switch
        {
            PlatformTypeEnum.Yellow => "yellow",
            PlatformTypeEnum.Blue => "blue",
            PlatformTypeEnum.FactoryGenerated => "factory_generated",
            _ => "unknown"
        };

        _metricsManager.OnPlatformSteppedOn(platformId, typeString);
        Debug.Log($"[TrackedPlatform] Player stepped on {platformId} ({typeString}) at {Time.time}");
    }

    public void ResetTracking()
    {
        // Resetear el cooldown para el pr?ximo run
        _lastEventTime = -999f;
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

        var col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.DrawWireCube(transform.position, col.bounds.size);
        }
    }
}
