using UnityEngine;

[CreateAssetMenu(fileName = "ParabolicMovement", menuName = "BulletMovement/ParabolicMovement")]
public class ParabolicMovement : BulletMovementStrategy
{
    public float speed = 5f;
    public float height = 5f;
    private Vector3 startPoint;
    private Vector3 midPoint;
    private Vector3 endPoint;
    private float journeyLength;
    private float startTime;

    /// <summary>
    /// Sets the start, mid, and end points of a parabolic trajectory
    /// Initializes the journey length and start time
    /// </summary>
    public override void Initialize(Vector3 targetPosition, Vector3 startPosition)
    {
        startPoint = startPosition;
        endPoint = targetPosition;

        midPoint = (startPoint + endPoint) / 2;
        midPoint.y += height;

        journeyLength = Vector3.Distance(startPoint, endPoint);
        startTime = Time.time;
    }

    /// <summary>
    /// Moves the bullet along a parabolic trajectory toward the target
    /// Destroys the bullet when it reaches the target
    /// </summary>
    public override void Move(Transform bulletTransform, Vector3 targetPosition)
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        bulletTransform.position = ParabolicTrajectory(startPoint, midPoint, endPoint, fractionOfJourney);

        if (fractionOfJourney >= 1)
        {
            Destroy(bulletTransform.gameObject);
        }
    }
    /// <summary>
    /// Calculates a parabolic point on the trajectory using quadratic interpolation.
    /// </summary>
    /// <returns>The interpolated position along the parabola.</returns>
    private Vector3 ParabolicTrajectory(Vector3 start, Vector3 middle, Vector3 end, float t)
    {
        Vector3 m1 = Vector3.Lerp(start, middle, t);
        Vector3 m2 = Vector3.Lerp(middle, end, t);
        return Vector3.Lerp(m1, m2, t);
    }
}