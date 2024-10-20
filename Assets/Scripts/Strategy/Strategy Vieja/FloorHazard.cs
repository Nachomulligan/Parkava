using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHazard : MonoBehaviour
{
    [SerializeField] private float tickDamage;

    private void OnTriggerStay(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(tickDamage * Time.fixedDeltaTime);
        }
    }
}
