using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource sfxSource; //For sound effects
    private AudioSource musicSource; //For background music
    private Dictionary<string, AudioClip> soundClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //Initialize audio
        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.enabled = true;
        soundClips = new Dictionary<string, AudioClip>();
    }

    // Load an audio clip from Resource folder
    public void LoadSound(string name, string filePath)
    {
        AudioClip clip = Resources.Load<AudioClip>(filePath);
        if (clip != null)
        {
            soundClips[name] = clip;
            Debug.Log($"Loaded sound: {name}");
        }
        else
        {
            Debug.Log($"Failed to load sound at path: {filePath}");
        }
    }
    
    // Play sound effect 
    public void PlaySound(string name)
    {
        if (soundClips.ContainsKey(name))
        {
            sfxSource.PlayOneShot(soundClips[name]);
        }
        else
        {
            Debug.Log($"Sound {name} not found!");
        }
    }

    // Play background music
    public void PlayMusic(string name)
    {
        if (soundClips.ContainsKey(name))
        {
            musicSource.clip = soundClips[name];
            musicSource.Play();
        }
        else
        {
            Debug.Log($"Music {name} not found!");
        }
    }

    // Stop all sound effects
    public void StopSound()
    {
        sfxSource.Stop();
    }

    // Stop background music
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Set volume for sound effects
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }

    // Set volume for background music
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }
}
