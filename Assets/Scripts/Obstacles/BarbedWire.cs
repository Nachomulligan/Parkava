using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbedWire : MonoBehaviour
{
    [SerializeField] private int Damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var character = ServiceLocator.Instance.GetService(nameof(Character)) as Character;
            if (character != null && other.gameObject == character.gameObject)
            {
                character.health.TakeDamage(Damage);
            }
        }
    }
}