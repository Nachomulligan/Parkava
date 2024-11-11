using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : IState
{
    public string Id { get; private set; } = "Pause";
    public Dictionary<string, IState> Outputs { get; private set; }

    public PauseState()
    {
        Outputs = new Dictionary<string, IState>();
    }

    public void Enter()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Entered Pause State");
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        Debug.Log("Exiting Pause State");
    }
}
