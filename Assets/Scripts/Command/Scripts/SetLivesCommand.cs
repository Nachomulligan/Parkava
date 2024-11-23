using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Set Player Lives")]
public class SetLivesCommand : Command
{
    public override void Execute(string[] args)
    {
        if (args.Length == 0 || !int.TryParse(args[0], out int lives))
        {
            Debug.LogError($"{Name}: Invalid or missing argument. Usage: <Lives Value>");
            return;
        }

        var lifeService = ServiceLocator.Instance.GetService(nameof(LifeService)) as LifeService;
        if (lifeService != null)
        {
            lifeService.SetLives(lives);
            Debug.Log($"{Name}: Player lives set to {lives}");
        }
        else
        {
            Debug.LogError($"{Name}: LifeService not found.");
        }
    }
}
