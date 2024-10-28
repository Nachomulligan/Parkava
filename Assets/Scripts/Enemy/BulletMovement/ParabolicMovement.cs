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

    public override void Initialize(Vector3 targetPosition, Vector3 startPosition)
    {
        startPoint = startPosition;
        endPoint = targetPosition;

        // Calculamos el punto medio con una elevación
        midPoint = (startPoint + endPoint) / 2;
        midPoint.y += height;

        // Almacenamos la longitud del recorrido y el tiempo inicial
        journeyLength = Vector3.Distance(startPoint, endPoint);
        startTime = Time.time;
    }

    public override void Move(Transform bulletTransform, Vector3 targetPosition)
    {
        // Calculamos el tiempo pasado
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        // Movimiento parabólico
        bulletTransform.position = ParabolicTrajectory(startPoint, midPoint, endPoint, fractionOfJourney);

        // Opcional: Si quieres que la bala se destruya al llegar al objetivo
        if (fractionOfJourney >= 1)
        {
            Destroy(bulletTransform.gameObject);
        }
    }

    private Vector3 ParabolicTrajectory(Vector3 start, Vector3 middle, Vector3 end, float t)
    {
        // Usamos interpolación cuadrática para la parábola
        Vector3 m1 = Vector3.Lerp(start, middle, t);
        Vector3 m2 = Vector3.Lerp(middle, end, t);
        return Vector3.Lerp(m1, m2, t);
    }
}
