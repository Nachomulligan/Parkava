using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.GetCurrentState() is GameplayState)
            {
                Pause();
            }
            else if (GameManager.Instance.GetCurrentState() is PauseState)
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameManager.Instance.GoToPauseMenu();  // Cambio de estado a Pause
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameManager.Instance.GoToGameplay();  // Cambio de estado a Gameplay
    }
}
