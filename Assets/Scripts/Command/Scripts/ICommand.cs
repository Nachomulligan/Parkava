using System.Collections;
using System.Collections.Generic;
using System.IO;

public interface ICommand
{
    public string Name { get; }
    public void Execute(params string[] args);
    public List<string> Aliases { get; }
}
