﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletMovementStrategy movementStrategy;
    private Vector3 targetPosition;
    private float lifetime = 5f;
    private float lifeTimer;
    private int damage = 1;
    public void SetMovementStrategy(BulletMovementStrategy strategy)
    {
        movementStrategy = strategy;
    }

    public void SetTarget(Transform target)
    {
        targetPosition = target.position;
        lifeTimer = 0f;

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

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null)
        {
            character.health.TakeDamage(damage);

            Destroy(gameObject);
        }
    }

}