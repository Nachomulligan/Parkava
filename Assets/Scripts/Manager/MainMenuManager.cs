using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private SceneController _sceneController;

    private void Awake()
    {
        _sceneController = FindObjectOfType<SceneController>();
        if (_sceneController == null)
        {
            Debug.LogError("SceneController not found in the scene.");
        }
    }

    public void QuitGame()
    {
        if (_sceneController != null)
        {
            _sceneController.QuitGame();
        }
    }

    public void LoadScene(string sceneName)
    {
        if (_sceneController != null)
        {
            _sceneController.LoadSceneByName(sceneName);
        }
    }
}

