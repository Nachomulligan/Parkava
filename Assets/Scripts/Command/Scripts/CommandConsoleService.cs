using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConsoleService : MonoBehaviour
{
    [SerializeField] private List<Command> commands;
    private Dictionary<string, ICommand> commandDictionary = new();

    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(CommandConsoleService), this);

        foreach (var command in commands)
        {
            AddCommand(command);
        }
    }

    public void AddCommand(ICommand command)
    {
        if (!commandDictionary.TryAdd(command.Name, command))
        {
            Debug.LogWarning($"Command '{command.Name}' already exists in the dictionary.");
        }

        foreach (var alias in command.Aliases)
        {
            if (!commandDictionary.TryAdd(alias, command))
            {
                Debug.LogWarning($"Alias '{alias}' for command '{command.Name}' already exists.");
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
