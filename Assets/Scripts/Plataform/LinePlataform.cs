using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePlataform : MonoBehaviour
{
    private float speed;

    public void Initialize(float platformSpeed)
    {
        speed = platformSpeed;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
