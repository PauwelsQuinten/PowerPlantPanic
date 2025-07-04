using UnityEditor;
using UnityEngine;

public class GamePlaySounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip ambience;
    [SerializeField]
    private SoundManager _soundManager;

    private void Start()
    {
        _soundManager.LoadSoundWithOutPath("ambiantSound", ambience);

        _soundManager.PlayMusic("ambiantSound");
    }
}
