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

    private LifeService lifeService;
    private PlayerMovement playerMovement;

    private void Start()
    {
        lifeService = ServiceLocator.Instance.GetService(nameof(LifeService)) as LifeService;
        if (lifeService == null)
        {
            Debug.LogError("LifeService no encontrado en el ServiceLocator.");
            return;
        }
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement no encontrado en la escena.");
            return;
        }

        lifeService.OnLifeLost += UpdateLivesUI;

        UpdateLivesUI();
        UpdateJumpsUI();
    }

    private void Update()
    {
        UpdateJumpsUI();
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

    private void OnDestroy()
    {
        if (lifeService != null)
        {
            lifeService.OnLifeLost -= UpdateLivesUI;
        }
    }
}