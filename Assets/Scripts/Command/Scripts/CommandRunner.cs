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

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    /// <summary>
    /// Get CommandConsoleService from ServiceLocator on every click an do the exucute command
    /// </summary>
    private void HandleClick()
    {
        var commandConsoleService = ServiceLocator.Instance.GetService(nameof(CommandConsoleService)) as CommandConsoleService;

        if (commandConsoleService != null)
        {
            string[] args = arguments.Split(' ');
            commandConsoleService.ExecuteCommand(commandAlias, args);
        }
        else
        {
            Debug.LogError($"{nameof(CommandConsoleService)} service not found in the ServiceLocator!");
        }
    }
}
