using UnityEngine;

[CreateAssetMenu(fileName = "StraightMovement", menuName = "BulletMovement/StraightMovement")]
public class StraightMovement : BulletMovementStrategy
{
    public float speed = 10f;
    private Vector3 startPosition;
    private Vector3 direction;

    /// <summary>
    /// Sets the starting position and direction towards the target
    /// </summary>
    public override void Initialize(Vector3 targetPosition, Vector3 startPosition)
    {
        this.startPosition = startPosition;
        direction = (targetPosition - startPosition).normalized;
    }

    /// <summary>
    /// Moves the bullet in a straight line toward the target at a constant speed
    /// Destroys the bullet when it reaches the target
    /// </summary>
    public override void Move(Transform bulletTransform, Vector3 targetPosition)
    {
        bulletTransform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(bulletTransform.position, targetPosition) < 0.1f)
        {
            Destroy(bulletTransform.gameObject);
        }
    }
}