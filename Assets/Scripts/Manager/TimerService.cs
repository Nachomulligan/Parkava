using System;
using UnityEngine;

public class TimerService : MonoBehaviour
{
    private float sessionTime = 0f;
    private float runTime = 0f;
    private bool isRunTimerActive = false;

    public event Action<float> OnRunTimeUpdated = delegate { };
    public event Action<float> OnSessionTimeUpdated = delegate { };

    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(TimerService), this);
    }

    private void Update()
    {
        sessionTime += Time.unscaledDeltaTime;
        OnSessionTimeUpdated?.Invoke(sessionTime);

        if (isRunTimerActive)
        {
            runTime += Time.deltaTime;
            OnRunTimeUpdated?.Invoke(runTime);
        }
    }
    public void StartRun()
    {
        runTime = 0f;
        isRunTimerActive = true;
        Debug.Log("Run Timer started");
    }
    public void PauseRun()
    {
        isRunTimerActive = false;
        Debug.Log($"Run Timer paused at {FormatTime(runTime)}");
    }
    public void ResumeRun()
    {
        isRunTimerActive = true;
        Debug.Log("Run Timer resumed");
    }
    public float GetSessionTime() => sessionTime;

    public float GetRunTime() => runTime;
    public static string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000f) % 1000f);
        
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
    public string GetFormattedSessionTime()
    {
        return FormatTime(sessionTime);
    }
    public string GetFormattedRunTime()
    {
        return FormatTime(runTime);
    }
}
