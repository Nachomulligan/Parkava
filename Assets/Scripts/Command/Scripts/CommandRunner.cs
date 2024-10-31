using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CommandRunner : MonoBehaviour
{
    [SerializeField] private string commandAlias;
    [SerializeField] private string arguments;
    private Button _button;
    private CommandConsoleManager _commandConsoleManager;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _commandConsoleManager = FindObjectOfType<CommandConsoleManager>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        if (_commandConsoleManager != null)
        {
            string[] args = arguments.Split(' ');
            _commandConsoleManager.ExecuteCommand(commandAlias, args);
        }
        else
        {
            Debug.LogError($"{nameof(CommandConsoleManager)} not found in the scene!");
        }
    }
}
