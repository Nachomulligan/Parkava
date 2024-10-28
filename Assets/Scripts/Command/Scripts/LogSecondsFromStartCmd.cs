using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Log Seconds from start")]
public class LogSecondsFromStartCmd : Command
{
    public override void Execute()
    {
        Debug.Log(Time.realtimeSinceStartup);
    }

    public override void Execute(string[] args)
    {
        Debug.Log(Time.realtimeSinceStartup);
    }
}
