using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public GameObject pauseMenuUI;

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
        GameManager.Instance.GoToPauseMenu();  
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameManager.Instance.GoToGameplay(); 
    }
}
