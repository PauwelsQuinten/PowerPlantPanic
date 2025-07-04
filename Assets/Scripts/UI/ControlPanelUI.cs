using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.GPUSort;

public class ControlPanelUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _ui;
    [SerializeField]
    private TextMeshProUGUI _powerEfficiency;
    [SerializeField]
    private TextMeshProUGUI _fanRPM;
    [SerializeField]
    private GameObject _pressureNeedle;
    [SerializeField]
    private Image _wasteLight;
    [SerializeField]
    private GameEvent _gameLost;

    [Header("Sound Variables")]
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private SoundManager _soundManager;

    private void Start()
    {
        _soundManager.LoadSoundWithOutPath("explosion", _explosionSound);
    }

    public void EnableUi(Component sender, object obj)
    {
        bool? setActive = obj as bool?;
        if((bool)setActive) _ui.SetActive(true);
        else _ui.SetActive(false);
    }

    public void PowerEfficiencyChanged(Component sender, object obj)
    {
        PowerEfficiencyChangedEventArgs args = obj as PowerEfficiencyChangedEventArgs;
        if (args == null) return;

        _powerEfficiency.text = $"{args.PowerEfficiency} %";
    }

    public void FanRPMChanged(Component sender, object obj)
    {
        FanRPMChangedEventArgs args = obj as FanRPMChangedEventArgs;
        if (args == null) return;

        _fanRPM.text = args.FanRPM.ToString();
    }

    public void PipePressureChanged(Component sender, object obj)
    {
        PipePresureEventArgs args = obj as PipePresureEventArgs;
        if (args == null) return;

        float newAngle = (args.PiperPressure / 150f  * 135f - 90f) * -1f;
        _pressureNeedle.transform.eulerAngles = new Vector3(0, 0, newAngle);
    }

    public void WasteTimerChanged(Component sender, object obj)
    {
        WasteTimerChangedEventArgs args = obj as WasteTimerChangedEventArgs;
        if (args == null) return;

        if (_wasteLight.color == Color.white)
            _wasteLight.color = Color.red;
        else _wasteLight.color = Color.white;

        if(args.WasteTimer == 100) _wasteLight.color = Color.white;
    }

    public void GiveUp()
    {
        _soundManager.PlaySound("explosion");

        _gameLost.Raise(this, EventArgs.Empty);
    }
}
