using UnityEngine;

/// <summary>
/// Componente para objetos letales que causan muerte instantanea.
/// Establece la causa de muerte antes de matar al jugador.
/// </summary>
[RequireComponent(typeof(Collider))]
public class LethalObject : MonoBehaviour
{
    [Header("Death Cause")]
    [SerializeField] private DeathCauseType deathCause = DeathCauseType.Lava;

    [Header("Settings")]
    [SerializeField] private bool isTrigger = true;

    public enum DeathCauseType
    {
        Lava,
        Abyss,
        LethalWall,
        Pendulum,
        EnemyShot
    }

    private void Start()
    {
        var col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = isTrigger;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTrigger)
        {
            HandlePlayerContact(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isTrigger)
        {
            HandlePlayerContact(collision.gameObject);
        }
    }

    private void HandlePlayerContact(GameObject playerObject)
    {
        if (!playerObject.CompareTag("Player")) return;

        var character = playerObject.GetComponent<Character>();
        if (character == null) return;

        // Establecer la causa de muerte
        string causeString = deathCause switch
        {
            DeathCauseType.Lava => "lava",
            DeathCauseType.Abyss => "abyss",
            DeathCauseType.LethalWall => "lethal_wall",
            DeathCauseType.Pendulum => "pendulum",
            DeathCauseType.EnemyShot => "enemy_shot",
            _ => "unknown"
        };

        character.SetDeathCause(causeString);

        // Causar da?o letal
        if (character.health != null)
        {
            character.health.TakeDamage(9999);
        }

        Debug.Log($"[LethalObject] Player killed by {causeString}");
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = deathCause switch
        {
            DeathCauseType.Lava => new Color(1f, 0.3f, 0f, 0.5f),
            DeathCauseType.Abyss => new Color(0f, 0f, 0f, 0.5f),
            DeathCauseType.LethalWall => new Color(1f, 0f, 0f, 0.5f),
            DeathCauseType.Pendulum => new Color(0.5f, 0f, 0.5f, 0.5f),
            DeathCauseType.EnemyShot => new Color(1f, 1f, 0f, 0.5f),
            _ => Color.white
        };

        Gizmos.color = gizmoColor;
        
        var col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.DrawCube(transform.position, col.bounds.size);
        }
    }
}
