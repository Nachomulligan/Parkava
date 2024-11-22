using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState : IState
{
    private string _loseSceneName = "LoseScreen";

    public string Id { get; private set; } = "LoseState";
    public Dictionary<string, IState> Outputs { get; private set; }

    public LoseState()
    {
        Outputs = new Dictionary<string, IState>();
    }

    public void Enter()
    {
        Debug.Log("Entered Lose State");

        var sceneController = Object.FindObjectOfType<SceneController>();
        if (sceneController != null)
        {
            sceneController.LoadSceneByName(_loseSceneName);
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
        Debug.Log("Exiting Lose State");
    }
}
