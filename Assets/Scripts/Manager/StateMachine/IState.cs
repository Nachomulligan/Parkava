using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState
{
    Dictionary<string, IState> Outputs { get; }
    string Id { get; }
    void Enter();
    void Execute();
    void Exit();
}
