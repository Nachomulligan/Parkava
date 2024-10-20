using UnityEngine;

[CreateAssetMenu(fileName = "SineWaveMovement", menuName = "BulletMovement/SineWaveMovement")]
public class SineWaveMovement : BulletMovementStrategy
{
    public float speed = 5f;           // Velocidad de la bala
    public float oscillations = 2f;    // Número de crestas y valles en el recorrido
    public float heightFactor = 1f;    // Factor que controla la altura de la onda

    private Vector3 startPosition;
    private Vector3 direction;
    private float startTime;
    private float distanceToTarget;

    public override void Initialize(Vector3 targetPosition, Vector3 startPosition)
    {
        this.startPosition = startPosition;
        direction = (targetPosition - startPosition).normalized;
        distanceToTarget = Vector3.Distance(startPosition, targetPosition);

        // Evitar división por cero o valores muy pequeños
        if (distanceToTarget < 0.1f)
        {
            distanceToTarget = 0.1f; // Asignar un valor mínimo para evitar problemas de NaN
        }

        startTime = Time.time;
    }

    public override void Move(Transform bulletTransform, Vector3 targetPosition)
    {
        // Tiempo transcurrido desde el inicio
        float elapsedTime = Time.time - startTime;

        // Calcular cuánto de la distancia total ha recorrido
        float distanceCovered = elapsedTime * speed;
        float fractionOfJourney = distanceCovered / distanceToTarget;

        // Limitar el valor de fractionOfJourney entre 0 y 1
        fractionOfJourney = Mathf.Clamp01(fractionOfJourney);

        // Interpolación lineal entre startPosition y targetPosition
        Vector3 interpolatedPosition = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

        // Calcular la amplitud de la onda basada en la altura que desees
        float currentAmplitude = heightFactor * (distanceToTarget / 2);

        // Aplicar la onda seno en la dirección perpendicular al movimiento
        float sineWaveOffset = Mathf.Sin(fractionOfJourney * Mathf.PI * oscillations) * currentAmplitude;

        // Calcular la dirección perpendicular a la trayectoria
        Vector3 perpendicularDirection = Vector3.Cross(direction, Vector3.up).normalized;

        // Ajustar la posición de la bala con el offset del seno
        bulletTransform.position = interpolatedPosition + perpendicularDirection * sineWaveOffset;

        // Destruir la bala cuando llega al objetivo
        if (fractionOfJourney >= 1)
        {
            Destroy(bulletTransform.gameObject);
        }
    }
}
