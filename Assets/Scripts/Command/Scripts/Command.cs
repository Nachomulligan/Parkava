using System.Collections.Generic;
using UnityEngine;

public abstract class Command : ScriptableObject, ICommand
{
    [field: SerializeField] public List<string> Aliases { get; private set; }

    public string Name => name;

    public abstract void Execute(params string[] args);
}
