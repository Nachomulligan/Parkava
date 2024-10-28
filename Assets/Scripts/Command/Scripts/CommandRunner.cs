using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CommandRunner : MonoBehaviour
{
    [SerializeField] private Command command;
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

    private void HandleClick()
    {
        if(command != null)
        {
            string[] args = arguments.Split(' ');
            command.Execute(args);
        }
        else
            Debug.LogError($"{name}: {nameof(command)} is null!", gameObject);
    }
}
