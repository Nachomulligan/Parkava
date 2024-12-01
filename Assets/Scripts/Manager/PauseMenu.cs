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
        _sceneController = ServiceLocator.Instance.GetService(nameof(SceneController)) as SceneController;
        if (_sceneController == null)
        {
            Debug.LogError($"{nameof(SceneController)} not found in the ServiceLocator.");
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
        var sceneController = ServiceLocator.Instance.GetService(nameof(SceneController)) as SceneController;
        if (sceneController != null)
        {
            sceneController.LoadSceneByName(sceneName);
        }
    }
    public void QuitGame()
    {
        var sceneController = ServiceLocator.Instance.GetService(nameof(SceneController)) as SceneController;
        if (sceneController != null)
        {
            sceneController.QuitGame();
        }
    }
}
