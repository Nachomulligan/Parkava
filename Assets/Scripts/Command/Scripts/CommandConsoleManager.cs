using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConsoleManager : MonoBehaviour
{
    [SerializeField] private List<Command> commands;
    private Dictionary<string, Command> commandDictionary;

    private void Awake()
    {
        commandDictionary = new Dictionary<string, Command>();

        foreach (var command in commands)
        {
           
            if (!commandDictionary.ContainsKey(command.Name))
            {
                commandDictionary.Add(command.Name, command);
            }

            foreach (var alias in command.Aliases)
            {
                if (!commandDictionary.ContainsKey(alias))
                {
                    commandDictionary.Add(alias, command);
                }
            }
        }
    }

    public void ExecuteCommand(string alias, string[] args)
    {
        if (commandDictionary.TryGetValue(alias, out Command command))
        {
            command.Execute(args);
        }
        else
        {
            Debug.LogError($"Command '{alias}' not found!");
        }
    }
}

