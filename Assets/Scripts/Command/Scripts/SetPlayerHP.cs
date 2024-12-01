using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Set Player HP")]
public class SetPlayerHPCommand : Command
{
    public override void Execute(string[] args)
    {
        if (args.Length == 0 || !int.TryParse(args[0], out int hp))
        {
            Debug.LogError($"{Name}: Invalid or missing argument. Usage: <HP Value>");
            return;
        }

        Character character = ServiceLocator.Instance.GetService(nameof(Character)) as Character;
        if (character != null && character.health != null)
        {
            character.SetHP(hp);
            Debug.Log($"{Name}: Player HP set to {hp}");
        }
        else
        {
            Debug.LogError($"{Name}: Character or health component not found.");
        }
    }
}