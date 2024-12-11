using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScalableFactory<T>
{
    void UpdateScale(T obj);
}