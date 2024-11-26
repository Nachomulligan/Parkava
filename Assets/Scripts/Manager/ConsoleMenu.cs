using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleMenu : MonoBehaviour
{
    [SerializeField] private GameObject consoleMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
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
        consoleMenuUI.SetActive(true);
        GameManager.Instance.GoToPauseMenu(); 
    }

    public void Resume()
    {
        consoleMenuUI.SetActive(false);
        GameManager.Instance.GoToGameplay(); 
    }
}
