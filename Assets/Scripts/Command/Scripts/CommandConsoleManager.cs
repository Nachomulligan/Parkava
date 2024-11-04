using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConsoleManager : MonoBehaviour
{
    [SerializeField] private List<Command> commands;
    private Dictionary<string, ICommand> commandDictionary = new();

    private void Awake()
    {
        foreach (var command in commands)
        {
            AddCommand(command);
        }
    }

    public void AddCommand(ICommand command)
    {
        if (!commandDictionary.TryAdd(command.Name, command))
        {
            //log che mira que tenes duplicados
        }

        foreach (var alias in command.Aliases)
        {
            if (!commandDictionary.TryAdd(alias, command))
            {
                //log che mira que tenes duplicados
            }
        }
    }

    public void RemoveCommand(ICommand command)
    {
        commandDictionary.Remove(command.Name);
        foreach (var alias in command.Aliases)
        {
            commandDictionary.Remove(alias);
        }
    }

    public void ExecuteCommand(string alias, params string[] args)
    {
        if (commandDictionary.TryGetValue(alias, out ICommand command))
        {
            command.Execute(args);
        }
        else
        {
            Debug.LogError($"Command '{alias}' not found!");
        }
    }
}

