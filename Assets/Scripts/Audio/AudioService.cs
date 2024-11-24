using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioService : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; 
    [SerializeField] private AudioSource sfxSource; 

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClipEntry> audioClipEntries = new(); 

    private Dictionary<string, AudioClip> audioClips = new();

    private void Awake()
    {
        ServiceLocator.Instance.SetService(nameof(AudioService), this);
        InitializeClips();
    }

    private void InitializeClips()
    {
        foreach (var entry in audioClipEntries)
        {
            if (!audioClips.ContainsKey(entry.key))
            {
                audioClips[entry.key] = entry.clip;
            }
            else
            {
                Debug.LogWarning($"Duplicate audio key '{entry.key}' detected. Only the first entry will be used.");
            }
        }
    }
    public void PlaySFX(string clipKey)
    {
        if (audioClips.TryGetValue(clipKey, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError($"SFX with key '{clipKey}' not found.");
        }
    }

    public void PlayMusic(string clipKey)
    {
        if (audioClips.TryGetValue(clipKey, out var clip))
        {
            if (musicSource.clip != clip)
            {
                musicSource.Stop();
                musicSource.clip = clip;
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogError($"Music with key '{clipKey}' not found.");
        }
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
