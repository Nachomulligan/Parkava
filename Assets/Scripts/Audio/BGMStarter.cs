using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMStarter : MonoBehaviour
{
    [SerializeField] private string songKey; 
    void Start()
    {
        var audioService = ServiceLocator.Instance.GetService(nameof(AudioService)) as AudioService;
        if (audioService != null)
        {
            audioService.PlayMusic(songKey);

        }
    }
}
