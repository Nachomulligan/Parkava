using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    private SceneController _sceneController;
    private PlayerDataManager _playerDataManager;

    [Header("Player ID Settings")]
    [SerializeField] private TMP_InputField playerIdInputField;
    [SerializeField] private Button playButton;
    [SerializeField] private string gameplaySceneName = "Gameplay";

    [Header("UI Feedback (Optional)")]
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private GameObject playerIdPanel;

    private void Start()
    {
        _sceneController = ServiceLocator.Instance.GetService(nameof(SceneController)) as SceneController;
        if (_sceneController == null)
        {
            Debug.LogError($"{nameof(SceneController)} not found in the ServiceLocator.");
        }

        _playerDataManager = PlayerDataManager.Instance;

        if (_playerDataManager.IsPlayerIdSet())
        {
            Debug.Log($"Player ID already set: {_playerDataManager.GetPlayerId()}");
            if (playerIdPanel != null)
            {
                playerIdPanel.SetActive(false);
            }
            EnablePlayButton();
        }
        else
        {

            SetupPlayerIdInput();
        }
    }

    private void SetupPlayerIdInput()
    {

        if (playButton != null)
        {
            playButton.interactable = false;
        }

        if (playerIdInputField != null)
        {
            playerIdInputField.onValueChanged.AddListener(OnPlayerIdInputChanged);

            OnPlayerIdInputChanged(playerIdInputField.text);
        }
        else
        {
            Debug.LogError("Player ID Input Field is not assigned in the inspector!");
        }

        if (errorText != null)
        {
            errorText.gameObject.SetActive(false);
        }
    }

    private void OnPlayerIdInputChanged(string inputValue)
    {
        bool isValid = _playerDataManager.ValidatePlayerId(inputValue);

        if (playButton != null)
        {
            playButton.interactable = isValid;
        }

        if (errorText != null)
        {
            if (!isValid && !string.IsNullOrEmpty(inputValue))
            {
                errorText.text = "Please enter a valid Player ID";
                errorText.gameObject.SetActive(true);
            }
            else
            {
                errorText.gameObject.SetActive(false);
            }
        }
    }

    public void OnPlayButtonPressed()
    {
        if (_playerDataManager.IsPlayerIdSet())
        {
            LoadGameplayScene();
            return;
        }

        if (playerIdInputField != null)
        {
            string inputValue = playerIdInputField.text.Trim();

            if (_playerDataManager.ValidatePlayerId(inputValue))
            {
                bool success = _playerDataManager.SetPlayerId(inputValue);
                
                if (success)
                {
                    Debug.Log($"Player ID '{inputValue}' saved successfully!");
                    
                    LoadGameplayScene();
                }
                else
                {
                    ShowError("Failed to set Player ID. Please try again.");
                }
            }
            else
            {
                ShowError("Please enter a valid Player ID before playing.");
            }
        }
        else
        {
            Debug.LogError("Player ID Input Field is not assigned!");
        }
    }

    private void LoadGameplayScene()
    {
        if (_sceneController != null)
        {
            _sceneController.LoadSceneByName(gameplaySceneName);
        }
        else
        {
            SceneManager.LoadScene(gameplaySceneName);
        }
    }

    private void EnablePlayButton()
    {
        if (playButton != null)
        {
            playButton.interactable = true;
        }
    }

    private void ShowError(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
        }
        Debug.LogWarning(message);
    }

    public void QuitGame()
    {
        var sceneController = ServiceLocator.Instance.GetService(nameof(SceneController)) as SceneController;
        if (sceneController != null)
        {
            sceneController.QuitGame();
        }
    }

    public void LoadScene(string sceneName)
    {
        var sceneController = ServiceLocator.Instance.GetService(nameof(SceneController)) as SceneController;
        if (sceneController != null)
        {
            sceneController.LoadSceneByName(sceneName);
        }
    }

    private void OnDestroy()
    {
        if (playerIdInputField != null)
        {
            playerIdInputField.onValueChanged.RemoveListener(OnPlayerIdInputChanged);
        }
    }
}

