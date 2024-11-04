using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Set Player Max Jump Count")]
public class SetMaxJumpCountCommand : Command
{
    public override void Execute()
    {
        Debug.LogError("This command requires, at least, 1 argument");
    }

    public override void Execute(string[] args)
    {
        if (args.Length < 1)
        {
            Debug.LogError("Not enough arguments provided. This command requires 1 argument.");
            return;
        }

        if (int.TryParse(args[0], out int jumpCount))
        {
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            if (player != null)
            {
                player.SetMaxJumpCount(jumpCount);
                Debug.Log($"{Name}: Max Jump Count set to {jumpCount}");
            }
            else
            {
                Debug.LogError($"{Name}: No PlayerMovement instance found in the scene.", this);
            }
        }
        else
        {
            Debug.LogError($"{Name}: {args[0]} is not an integer!", this);
        }
    }
}
