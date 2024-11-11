using UnityEngine;

[CreateAssetMenu(fileName = "SineWaveMovement", menuName = "BulletMovement/SineWaveMovement")]
public class SineWaveMovement : BulletMovementStrategy
{
    public float speed = 5f;
    public float oscillations = 2f;
    public float heightFactor = 1f;

    private Vector3 startPosition;
    private Vector3 direction;
    private float startTime;
    private float distanceToTarget;

    /// <summary>
    /// Sets the start position, calculates direction, distance, and starting time for sine wave movement
    /// </summary>
    public override void Initialize(Vector3 targetPosition, Vector3 startPosition)
    {
        this.startPosition = startPosition;
        direction = (targetPosition - startPosition).normalized;
        distanceToTarget = Vector3.Distance(startPosition, targetPosition);

        if (distanceToTarget < 0.1f)
        {
            distanceToTarget = 0.1f;
        }

        startTime = Time.time;
    }

    /// <summary>
    /// Moves the bullet in a sine wave pattern toward the target position
    /// Destroys the bullet when it reaches the target
    /// </summary>
    public override void Move(Transform bulletTransform, Vector3 targetPosition)
    {
        float elapsedTime = Time.time - startTime;
        float distanceCovered = elapsedTime * speed;
        float fractionOfJourney = Mathf.Clamp01(distanceCovered / distanceToTarget);

        Vector3 interpolatedPosition = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
        float currentAmplitude = heightFactor * (distanceToTarget / 2);
        float sineWaveOffset = Mathf.Sin(fractionOfJourney * Mathf.PI * oscillations) * currentAmplitude;
        Vector3 perpendicularDirection = Vector3.Cross(direction, Vector3.up).normalized;

        bulletTransform.position = interpolatedPosition + perpendicularDirection * sineWaveOffset;

        if (fractionOfJourney >= 1)
        {
            Destroy(bulletTransform.gameObject);
        }
    }
}