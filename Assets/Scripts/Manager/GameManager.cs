using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Gameplay,
        Pause
    }
    private GameState currentState;

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

        currentState = GameState.Gameplay;
    }
    public void SetState(GameState newState)
    {
        currentState = newState;
        HandleStateChange();
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }

    private void HandleStateChange()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            case GameState.Pause:
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }

}
