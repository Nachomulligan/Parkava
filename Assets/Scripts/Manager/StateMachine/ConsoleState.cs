using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleState : IState
{
    public string Id { get; private set; } = "Pause";
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
        Debug.Log("Entered Pause State");
    }

    public void Execute()
    {
        // Comportamiento durante el estado de Pausa (ejecutado en cada frame)
    }

    public void Exit()
    {
        Debug.Log("Exiting Pause State");
    }
}
