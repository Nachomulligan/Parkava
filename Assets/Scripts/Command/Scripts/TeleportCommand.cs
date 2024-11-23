using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Teleport Player")]
public class TeleportCommand : Command
{
    public override void Execute(string[] args)
    {
        if (args.Length < 3)
        {
            Debug.LogError($"{Name}: Invalid arguments. Usage: <X> <Y> <Z>");
            return;
        }

        if (float.TryParse(args[0], out float x) && float.TryParse(args[1], out float y) && float.TryParse(args[2], out float z))
        {
            Character character = FindObjectOfType<Character>();
            if (character != null)
            {
                character.transform.position = new Vector3(x, y, z);
                Debug.Log($"{Name}: Teleported player to ({x}, {y}, {z})");
            }
            else
            {
                Debug.LogError($"{Name}: Character not found.");
            }
        }
        else
        {
            Debug.LogError($"{Name}: Invalid coordinates. Usage: <X> <Y> <Z>");
        }
    }
}