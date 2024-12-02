using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Set Player Shield")]
public class SetPlayerShieldCommand : Command
{
    public override void Execute(string[] args)
    {
        if (args.Length == 0 || !int.TryParse(args[0], out int shield))
        {
            Debug.LogError($"{Name}: Invalid or missing argument. Usage: <Shield Value>");
            return;
        }

        Character character = ServiceLocator.Instance.GetService(nameof(Character)) as Character;
        if (character != null)
        {
            character.SetShield(shield);
            Debug.Log($"{Name}: Player shield set to {shield}");
        }
        else
        {
            Debug.LogError($"{Name}: Character not found in the ServiceLocator.");
        }
    }
}