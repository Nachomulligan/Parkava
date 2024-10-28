using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthSystem
{
    public interface IHealth
    {
        void TakeDamage(int damage);
        void Heal(int amount);
        int GetCurrentHealth();
        int GetMaxHealth();
        event Action OnDeath;
    }
}