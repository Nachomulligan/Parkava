using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LifeService : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    public event Action OnLifeLost = delegate { };
    public event Action OnPermadeath = delegate { };

    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(LifeService), this);
        currentLives = maxLives;
    }

    public void ReduceLife()
    {
        if (currentLives <= 0) return;

        currentLives--;
        OnLifeLost?.Invoke();

        if (currentLives == 0)
        {
            OnPermadeath?.Invoke();
        }
    }

    public int GetCurrentLives() => currentLives;

    public void ResetLives()
    {
        currentLives = maxLives;
    }
}