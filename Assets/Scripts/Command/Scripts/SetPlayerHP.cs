using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Set Player HP")]
public class SetPlayerHP : Command
{
    public override void Execute()
    {
        Debug.LogError("This command requires, at least, 1 argument");
    }

    public override void Execute(string[] args)
    {
        if (int.TryParse(args[0], out int hp))
        {
            Debug.Log($"{name}: HP = {hp}");
        }
        else
            Debug.LogError($"{name}: {args[0]} is not an integer!", this);
    }
}