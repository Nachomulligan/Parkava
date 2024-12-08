using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HealthSystem
{
    public class ShieldDecorator : IHealth
    {
        private readonly IHealth _baseHealth;
        private int _shield;

        public ShieldDecorator(IHealth baseHealth, int shield)
        {
            _baseHealth = baseHealth;
            _shield = shield;
        }
        public void SetShield(int shieldAmount)
        {
            _shield = shieldAmount;
        }

        /// <summary>
        /// shield absorbs damage first, then base health takes remaining damage
        /// </summary>
        /// <param name="damagePoints"></param>
        public void TakeDamage(int damagePoints)
        {
            if (damagePoints < 0) return;

            if (_shield > 0)
            {
                int remainingDamage = damagePoints - _shield;
                _shield = Mathf.Max(0, _shield - damagePoints);
                if (remainingDamage > 0)
                {
                    _baseHealth.TakeDamage(remainingDamage);
                }
            }
            else
            {
                _baseHealth.TakeDamage(damagePoints);
            }
        }

        public void Heal(int amount) => _baseHealth.Heal(amount);
        public int GetCurrentHealth() => _baseHealth.GetCurrentHealth();
        public int GetMaxHealth() => _baseHealth.GetMaxHealth();
        public event Action OnDeath
        {
            add => _baseHealth.OnDeath += value;
            remove => _baseHealth.OnDeath -= value;
        }
    }
}
