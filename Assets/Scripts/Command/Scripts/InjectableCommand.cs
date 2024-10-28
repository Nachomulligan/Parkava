using System;
using System.Collections.Generic;

public class InjectableCommand : ICommand
{
    public string Name {get; set;}
    public List<string> Aliases { get; set;}

    public Action DoExecute;

    public Action<string[]> DoExecuteWithArgs;

    public InjectableCommand(string name, Action doExecute, Action<string[]> doExecuteWithArgs)
    {
        Name = name;
        DoExecute = doExecute;
        DoExecuteWithArgs = doExecuteWithArgs;
    }

    public void Execute()
    {
        DoExecute?.Invoke();
    }

    public void Execute(string[] args)
    {
        DoExecuteWithArgs?.Invoke(args);
    }
}