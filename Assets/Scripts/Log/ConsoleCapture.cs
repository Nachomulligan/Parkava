using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleCapture : MonoBehaviour
{
    [SerializeField] private TMP_Text output;
    void Start()
    {
        var unityLogHandler = Debug.unityLogger.logHandler;
        Debug.unityLogger.logHandler = new CustomLogHandler(output, unityLogHandler);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Debug.Log("This is a log!!");
        if (Input.GetKeyDown(KeyCode.S))
            Debug.LogError("This is a error");
        if (Input.GetKeyDown(KeyCode.W))
            Debug.LogWarning("This is a Warning");

    }
}

public class CustomLogHandler : ILogHandler
{
    private TMP_Text _output;
    private ILogHandler _unityConsoleLogHandler;

    public CustomLogHandler(TMP_Text output, ILogHandler unityConsoleLogHandler)
    {
        _output = output;
        _unityConsoleLogHandler = unityConsoleLogHandler;
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        //error assert warning log exception
        string[] colors = new string[] { "red", "green", "yellow", "white", "purple"};
        var logColor = colors[(int)logType];
        _output.SetText($"{_output.text}\n<color={logColor}>{string.Format(format, args)}</color>");
        _unityConsoleLogHandler.LogFormat(logType, context, format, args);
    }
    public void LogException(Exception exception, UnityEngine.Object context)
    {
        _output.SetText(_output.text + exception.Message);
        _unityConsoleLogHandler.LogException(exception, context);
    }

   
}