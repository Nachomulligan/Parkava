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
        OnPermadeath += HandlePermadeath;
    }

    private void OnDestroy()
    {
        OnPermadeath -= HandlePermadeath;
    }

    public void ReduceLife()
    {
        if (currentLives <= 0) return;

        currentLives--;
        OnLifeLost?.Invoke();
        var audioService = ServiceLocator.Instance.GetService(nameof(AudioService)) as AudioService;
        if (audioService != null)
        {
            audioService.PlaySFX("Die");

        }

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

    private void HandlePermadeath()
    {
        Debug.Log("Game Over: Permadeath triggered.");

        var gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.GoToLoseState();
        }
        else
        {
            Debug.LogError("GameManager instance not found.");
        }
    }

    internal void SetLives(int value)
    {
        currentLives = value;
    }
}