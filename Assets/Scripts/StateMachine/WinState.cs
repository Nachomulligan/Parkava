using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WinState : IState
{
    private string _winSceneName = "WinScreen";

    public string Id { get; private set; } = "WinState";
    public Dictionary<string, IState> Outputs { get; private set; }

    public WinState()
    {
        Outputs = new Dictionary<string, IState>();
    }

    public void Enter()
    {
        Debug.Log("Entered Win State");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        var sceneController = Object.FindObjectOfType<SceneController>();
        if (sceneController != null)
        {
            sceneController.LoadSceneByName(_winSceneName);
        }
        else
        {
            Debug.LogError("SceneController not found in the scene.");
        }
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        Debug.Log("Exiting Win State");
    }
}
