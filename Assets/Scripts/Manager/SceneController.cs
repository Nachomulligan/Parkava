using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(SceneController), this);
    }

    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is empty or null.");
            return;
        }

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            Debug.Log($"Loading scene: {sceneName}");
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' cannot be loaded. Check if it's added to the Build Settings.");
        }
    }
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"Invalid scene index: {sceneIndex}. Check Build Settings.");
            return;
        }

        SceneManager.LoadScene(sceneIndex);
        Debug.Log($"Loading scene with index: {sceneIndex}");
    }
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
    public void ReloadCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        LoadSceneByName(currentScene);
    }
}
