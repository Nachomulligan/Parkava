using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private StateMachine _stateMachine = new();

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
        GoToGameplay();
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void ChangeState(IState newState)
    {
        _stateMachine.ChangeState(newState);
    }

    public IState GetCurrentState()
    {
        return _stateMachine.GetCurrentState();
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
    public void GoToLoseState()
    {
        ChangeState(new LoseState());
    }
    public void GoToWinState()
    {
        ChangeState(new WinState());
    }
}
