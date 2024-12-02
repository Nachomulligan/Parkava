using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFactory<T>
{
    protected float minScale;
    protected float maxScale;
    protected float scaleStep;

    public AbstractFactory(float minScale, float maxScale, float scaleStep)
    {
        this.minScale = minScale;
        this.maxScale = maxScale;
        this.scaleStep = scaleStep;
    }

    public abstract T Create(Vector3 position);

    public virtual T Create(Vector3 position, string type)
    {
        Debug.LogError("This method is not implemented in the base class.");
        return default;
    }

    public abstract void UpdateScale(T obj);
}