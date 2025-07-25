using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource SfxSource { get; private set; } //For sound effects
    public AudioSource MusicSource { get; private set; } //For background music

    private Dictionary<string, AudioClip> _soundClips;
    private List<AudioClip> _activeClips = new List<AudioClip>();

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
        SfxSource = gameObject.AddComponent<AudioSource>();
        MusicSource = gameObject.AddComponent<AudioSource>();
        MusicSource.enabled = true;
        _soundClips = new Dictionary<string, AudioClip>();
    }

    // Load an audio clip from Resource folder
    public void LoadSound(string name, string filePath)
    {
        AudioClip clip = Resources.Load<AudioClip>(filePath);
        if (clip != null)
        {
            _soundClips[name] = clip;
            Debug.Log($"Loaded sound: {name}");
        }
        else
        {
            Debug.Log($"Failed to load sound at path: {filePath}");
        }
    }

    public void LoadSoundWithOutPath(string name, AudioClip audio)
    {
        AudioClip clip = audio;
        if (clip != null)
        {
            _soundClips[name] = clip;
            Debug.Log($"Loaded sound: {name}");
        }
        else
        {
            Debug.Log($"Failed to load sound: {audio}");
        }
    }
    
    // Play sound effect 
    public void PlaySound(string name)
    {

        if (_soundClips.ContainsKey(name))
        {
            AudioClip clip = _soundClips[name];

            if (_activeClips.Contains(clip)) return;
            
            _activeClips.Add(clip);
            SfxSource.PlayOneShot(clip);

            StartCoroutine(RemoveClipAfterPlaying(clip, clip.length));
        }
        else
        {
            Debug.Log($"Sound {name} not found!");
        }
    }

    // Play background music
    public void PlayMusic(string name)
    {
        if (_soundClips.ContainsKey(name))
        {
            MusicSource.clip = _soundClips[name];
            MusicSource.Play();
        }
        else
        {
            Debug.Log($"Music {name} not found!");
        }
    }

    // Stop all sound effects
    public void StopSound()
    {
        SfxSource.Stop();
    }

    // Stop background music
    public void StopMusic()
    {
        MusicSource.Stop();
    }

    // Set volume for sound effects
    public void SetSFXVolume(float volume)
    {
        SfxSource.volume = Mathf.Clamp01(volume);
    }

    // Set volume for background music
    public void SetMusicVolume(float volume)
    {
        MusicSource.volume = Mathf.Clamp01(volume);
    }

    private IEnumerator RemoveClipAfterPlaying(AudioClip clip, float duration)
    {
        yield return new WaitForSeconds(duration);
        _activeClips.Remove(clip);
    }
}
