using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public GameObject pauseMenuUI;
    private SceneController _sceneController;
    private void Awake()
    {
        _sceneController = FindObjectOfType<SceneController>();
        if (_sceneController == null)
        {
            Debug.LogError("SceneController not found in the scene.");
        }
    }

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

    public void LoadScene(string sceneName)
    {
        if (_sceneController != null)
        {
            _sceneController.LoadSceneByName(sceneName);
        }
    }
    public void QuitGame()
    {
        if (_sceneController != null)
        {
            _sceneController.QuitGame();
        }
    }
}
