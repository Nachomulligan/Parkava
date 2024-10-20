using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletMovementStrategy movementStrategy;
    private Vector3 targetPosition;
    private float lifetime = 5f; // Tiempo máximo de vida
    private float lifeTimer;

    public void SetTarget(Transform target)
    {
        targetPosition = target.position;
        lifeTimer = 0f; // Reinicia el temporizador cuando se asigna un nuevo objetivo

        // Inicializa la estrategia de movimiento con la posición objetivo y la posición inicial
        if (movementStrategy != null)
        {
            movementStrategy.Initialize(targetPosition, transform.position);
        }
    }

    private void Update()
    {
        if (movementStrategy != null)
        {
            movementStrategy.Move(transform, targetPosition);
        }

        // Destruir la bala después de cierto tiempo si no alcanza el objetivo
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}