using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : IState
{
    public string Id { get; private set; } = "Gameplay";
    public Dictionary<string, IState> Outputs { get; private set; }

    public GameplayState()
    {
        // Definir los posibles estados a los que puede cambiar
        Outputs = new Dictionary<string, IState>();
    }

    public void Enter()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Entered Gameplay State");
    }

    public void Execute()
    {
        // Comportamiento durante el estado de Gameplay (ejecutado en cada frame)
    }

    public void Exit()
    {
        // Cualquier limpieza o lógica al salir del estado
        Debug.Log("Exiting Gameplay State");
    }
}
