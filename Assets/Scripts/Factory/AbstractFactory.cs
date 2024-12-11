using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFactory<T>
{
    public abstract T Create(Vector3 position);
}