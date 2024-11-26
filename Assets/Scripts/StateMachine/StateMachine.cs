using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<string, IState> statesById;
    private IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
    
    public void ChangeState(string stateId)
    {
        if (statesById.TryGetValue(stateId, out var newState))
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }

    public void Update()
    {
        currentState?.Execute();
    }

    public IState GetCurrentState()
    {
        return currentState;
    }
}
