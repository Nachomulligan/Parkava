using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Set Player Max Jump Count")]
public class SetMaxJumpCountCommand : Command
{
    public override void Execute(string[] args)
    {
        if (int.TryParse(args[0], out int jumpCount))
        {
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            if (player != null)
            {
                player.SetMaxJumpCount(jumpCount);
                Debug.Log($"{Name}: Max Jump Count set to {jumpCount}");
            }
        }
    }
}
