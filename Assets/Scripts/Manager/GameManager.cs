using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IState currentState; // Estado actual
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            if (_instance == null)
            {
                var newGO = new GameObject();
                _instance = newGO.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Inicialmente en estado de Gameplay
        ChangeState(new GameplayState());
    }

    private void Update()
    {
        // Ejecuta la lógica del estado actual en cada frame
        currentState?.Execute();
    }

    // Método para cambiar de estado
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    // Obtener el estado actual
    public IState GetCurrentState()
    {
        return currentState;
    }

    // Método para ir a Gameplay
    public void GoToGameplay()
    {
        ChangeState(new GameplayState());
    }

    // Método para ir a PauseMenu
    public void GoToPauseMenu()
    {
        ChangeState(new PauseState());
    }
}
