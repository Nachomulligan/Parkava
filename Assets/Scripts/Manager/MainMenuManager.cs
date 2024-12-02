using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private SceneController _sceneController;

    private void Start()
    {
        _sceneController = ServiceLocator.Instance.GetService(nameof(SceneController)) as SceneController;
        if (_sceneController == null)
        {
            Debug.LogError($"{nameof(SceneController)} not found in the ServiceLocator.");
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

    public void LoadScene(string sceneName)
    {
        var sceneController = ServiceLocator.Instance.GetService(nameof(SceneController)) as SceneController;
        if (sceneController != null)
        {
            sceneController.LoadSceneByName(sceneName);
        }
    }
}

