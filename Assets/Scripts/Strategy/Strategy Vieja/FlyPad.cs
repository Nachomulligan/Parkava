using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPad : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IFly>(out IFly fly))
        {
            fly.SetFlyState();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IWalk>(out IWalk walk))
        {
            walk.SetWalkState();
        }
    }
}
