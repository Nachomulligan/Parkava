using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Platform : MonoBehaviour
{
    protected float speed;
    protected Vector3 moveDirection = Vector3.right;

    public virtual void Initialize(float platformSpeed, Vector3 direction)
    {
        speed = platformSpeed;
        moveDirection = direction.normalized;
    }

    protected void MovePlatform()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public virtual void ResetPlatform()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                child.SetParent(null);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == transform)
        {
            other.transform.SetParent(null);
        }
    }
}
