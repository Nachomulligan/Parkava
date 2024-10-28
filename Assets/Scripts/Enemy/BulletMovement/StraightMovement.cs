using UnityEngine;

[CreateAssetMenu(fileName = "StraightMovement", menuName = "BulletMovement/StraightMovement")]
public class StraightMovement : BulletMovementStrategy
{
    public float speed = 10f;
    private Vector3 startPosition;
    private Vector3 direction;

    public override void Initialize(Vector3 targetPosition, Vector3 startPosition)
    {
        this.startPosition = startPosition;
        direction = (targetPosition - startPosition).normalized;
    }

    public override void Move(Transform bulletTransform, Vector3 targetPosition)
    {
        // Mover la bala en línea recta hacia el objetivo
        bulletTransform.position += direction * speed * Time.deltaTime;

        // Opcional: Destruir la bala cuando llega al objetivo
        if (Vector3.Distance(bulletTransform.position, targetPosition) < 0.1f)
        {
            Destroy(bulletTransform.gameObject);
        }
    }
}
