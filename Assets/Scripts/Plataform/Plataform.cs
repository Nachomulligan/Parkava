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
}
