using System;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private static PlayerDataManager _instance;
    
    private string _playerId = string.Empty;
    private bool _isPlayerIdSet = false;
    private int _runCount = 0;

    public static PlayerDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerDataManager>();
                
                if (_instance == null)
                {
                    GameObject go = new GameObject("PlayerDataManager");
                    _instance = go.AddComponent<PlayerDataManager>();
                }
            }
            return _instance;
        }
    }
    public int RunCount => _runCount;

    public event Action<string> OnPlayerIdSet = delegate { };
    public event Action<bool> OnPlayerIdValidationChanged = delegate { };

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        ServiceLocator.Instance.SetService(nameof(PlayerDataManager), this);
        
        Debug.Log("PlayerDataManager initialized and persisted across scenes.");
    }

    /// <summary>
    /// Establece el Player ID. Solo puede ser establecido una vez.
    /// </summary>
    /// <param name="playerId">El ID ?nico del jugador</param>
    /// <returns>True si se estableci? correctamente, false si ya estaba establecido</returns>
    public bool SetPlayerId(string playerId)
    {
        if (_isPlayerIdSet)
        {
            Debug.LogWarning($"Player ID already set to '{_playerId}'. Cannot change it.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(playerId))
        {
            Debug.LogError("Cannot set an empty Player ID.");
            return false;
        }

        _playerId = playerId.Trim();
        _isPlayerIdSet = true;
        
        Debug.Log($"Player ID set to: {_playerId}");
        OnPlayerIdSet?.Invoke(_playerId);
        
        return true;
    }

    public string GetPlayerId()
    {
        return _playerId;
    }

    public bool IsPlayerIdSet()
    {
        return _isPlayerIdSet;
    }

    public bool ValidatePlayerId(string playerId)
    {
        bool isValid = !string.IsNullOrWhiteSpace(playerId) && playerId.Trim().Length > 0;
        return isValid;
    }

    public void ResetPlayerId()
    {
        _playerId = string.Empty;
        _isPlayerIdSet = false;
        Debug.Log("Player ID has been reset.");
    }

    public void IncrementRunCount()
    {
        _runCount++;
        Debug.Log($"Run actual: {_runCount}");
    }

    public void ResetRunCount()
    {
        _runCount = 0;
        Debug.Log("Run count reseteado.");
    }
}
