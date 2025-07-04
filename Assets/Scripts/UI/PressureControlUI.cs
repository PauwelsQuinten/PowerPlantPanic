using System.Collections.Generic;
using UnityEngine;

public class PressureControlUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _ui;
    [Header("Audio Variable")]
    [SerializeField]
    private AudioClip _valveTurning;
    [SerializeField]
    private SoundManager _soundManager;

    private void Start()
    {
        _soundManager.LoadSoundWithOutPath("turning", _valveTurning);
    }

    public void EnableUI(Component sender, object obj)
    {
        bool? setActive = obj as bool?;
        if ((bool)setActive) _ui.SetActive(true);
        else _ui.SetActive(false);
    }

    public void ValveRotationChanged(Component sender, object obj)
    {
        ValveRotationChangedEventArgs args = obj as ValveRotationChangedEventArgs;

        if (args == null) return;

        _soundManager.PlaySound("turning");

        args.Valve.transform.eulerAngles = new Vector3(0, 0, args.ValveRotation * -1);
    }
}
