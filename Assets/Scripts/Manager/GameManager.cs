using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IState currentState; 
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

        ChangeState(new GameplayState());
    }

    private void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    public IState GetCurrentState()
    {
        return currentState;
    }

    public void GoToGameplay()
    {
        ChangeState(new GameplayState());
    }


    public void GoToPauseMenu()
    {
        ChangeState(new PauseState());
    }

    public void GoToConsoleMenu()
    {
        ChangeState(new ConsoleState());
    }
}
