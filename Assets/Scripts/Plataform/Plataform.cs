using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Platform : MonoBehaviour
{
    protected float speed;

    public virtual void Initialize(float platformSpeed)
    {
        speed = platformSpeed;
    }

    protected void MovePlatform()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}