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
            if (GameManager.Instance.GetCurrentState() == GameState.Gameplay)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameManager.Instance.SetState(GameState.Pause);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameManager.Instance.SetState(GameState.Gameplay);
    }
}