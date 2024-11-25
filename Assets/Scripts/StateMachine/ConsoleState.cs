using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleState : IState
{
    public string Id { get; private set; } = "Console";
    public Dictionary<string, IState> Outputs { get; private set; }

    public ConsoleState()
    {
        Outputs = new Dictionary<string, IState>();
    }

    public void Enter()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Entered Console State");
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        Debug.Log("Exiting Console State");
    }
}
