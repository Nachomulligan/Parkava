using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUDManager : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private TMP_Text livesText; 
    [SerializeField] private TMP_Text jumpsText;
    [SerializeField] private TMP_Text runTimerText;

    private LifeService lifeService;
    private PlayerMovement playerMovement;
    private TimerService timerService;

    private void Start()
    {
        lifeService = ServiceLocator.Instance.GetService(nameof(LifeService)) as LifeService;
        if (lifeService == null)
        {
            Debug.LogError("LifeService no encontrado en el ServiceLocator.");
            return;
        }
        playerMovement = ServiceLocator.Instance.GetService(nameof(PlayerMovement)) as PlayerMovement;
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement no encontrado en la escena.");
            return;
        }

        timerService = ServiceLocator.Instance.GetService(nameof(TimerService)) as TimerService;
        if (timerService == null)
        {
            Debug.LogError("TimerService no encontrado en el ServiceLocator.");
        }

        lifeService.OnLifeLost += UpdateLivesUI;

        UpdateLivesUI();
        UpdateJumpsUI();
    }

    private void Update()
    {
        UpdateJumpsUI();
        UpdateRunTimerUI();
    }

    private void UpdateLivesUI()
    {
        livesText.text = $"Lives: {lifeService.GetCurrentLives()}";
    }

    private void UpdateJumpsUI()
    {
        if (playerMovement != null)
        {
            jumpsText.text = $"Jumps: {playerMovement.currentJumpCount}";
        }
    }

    private void UpdateRunTimerUI()
    {
        if (timerService != null && runTimerText != null)
        {
            runTimerText.text = $"Time: {timerService.GetFormattedRunTime()}";
        }
    }

    private void OnDestroy()
    {
        if (lifeService != null)
        {
            lifeService.OnLifeLost -= UpdateLivesUI;
        }
    }
}