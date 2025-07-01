using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CentralControlPanel : MonoBehaviour
{
    [SerializeField]
    private GameEvent _openControlPanel;
    [SerializeField]
    private float _minigamesInterval;
    [SerializeField]
    private int _errorRate;
    [SerializeField]
    private List<GameEvent> _enableMiniGame = new List<GameEvent>();
    [SerializeField]
    private GameEvent _powerEfficiencyChanged;
    [SerializeField]
    private GameEvent _fanRPMChanged;
    [SerializeField]
    private GameEvent _pipePressureChanged;
    [SerializeField]
    private GameEvent _wasteTimerChanged;
    [SerializeField]
    private InputActionReference _closePanel;
    [SerializeField]
    private GameEvent _gameLost;
    [SerializeField]
    private int _powerDrainAmount = 1;
    [SerializeField]
    private int _pressureDrainAmount = 10;
    [SerializeField]
    private int _RPMDrainAmount = 1;
    [SerializeField]
    private int _accumulateWasteAmount = 1;
    [SerializeField]
    private float _powerDrainSpeed = 0.5f;
    [SerializeField]
    private float _pressureDrainSpeed = 1;
    [SerializeField]
    private float _RPMDrainSpeed = 1;
    [SerializeField]
    private float _accumulateWasteSpeed = 1;

    private int _lastEnabledMiniGame = -1;

    private int _powerEfficiency = 100;
    private int _fanRPM = 3600;
    private int _pipePSI = 150;
    private int _wasteTimer = 100;

    private Coroutine _decreaseOutputEfficiency;
    private Coroutine _decreaseFanRPM;
    private Coroutine _decreasePipePressure;
    private Coroutine _accumulateWaste;

    private bool _canDecreasePower = false;
    private bool _canDecreaseFanRPM = false;
    private bool _canDecreasePipePressure = false;
    private bool _canAccumulateWaste = false;


    private void Start()
    {
        StartCoroutine(SelectRandomMiniGame());
    }

    private IEnumerator SelectRandomMiniGame()
    {
        yield return new WaitForSeconds(_minigamesInterval);
        int spawnError = UnityEngine.Random.Range(1, _errorRate + 1);

        if (spawnError == 2)
        {
            int index = UnityEngine.Random.Range(0, _enableMiniGame.Count);

            if(_enableMiniGame.Count > 1)
            {
                while (index == _lastEnabledMiniGame)
                {
                    index = UnityEngine.Random.Range(0, _enableMiniGame.Count);
                }
            }

            if(index != _lastEnabledMiniGame) 
                _enableMiniGame[(int)index].Raise(this, EventArgs.Empty);

            _lastEnabledMiniGame = index;
        }

        StartCoroutine(SelectRandomMiniGame());
    }

    private IEnumerator DecreasePowerEfficiency()
    {
        if (_canDecreasePower)
        {
            yield return new WaitForSeconds(_powerDrainSpeed);

            _powerEfficiency -= _powerDrainAmount;
            if (_powerEfficiency <= 0) 
                _gameLost.Raise(this, EventArgs.Empty);
            _powerEfficiencyChanged.Raise(this, new PowerEfficiencyChangedEventArgs { PowerEfficiency = _powerEfficiency });
            _decreaseOutputEfficiency = StartCoroutine(DecreasePowerEfficiency());
        }
    }

    private IEnumerator DecreaseFanRPM()
    {
        yield return new WaitForSeconds(_RPMDrainSpeed);

        _fanRPM -= _RPMDrainAmount;

        if (_fanRPM <= 0)
            _gameLost.Raise(this, EventArgs.Empty);

        _fanRPMChanged.Raise(this, new FanRPMChangedEventArgs { FanRPM = _fanRPM });

        _decreaseFanRPM = StartCoroutine(DecreaseFanRPM());
    }

    private IEnumerator DecreasePipePressure()
    {
        yield return new WaitForSeconds(_pressureDrainSpeed);

        _pipePSI -= _pressureDrainAmount;

        if (_pipePSI <= 0)
            _gameLost.Raise(this, EventArgs.Empty);

        _pipePressureChanged.Raise(this, new PipePresureEventArgs { PiperPressure = _pipePSI });

        _decreasePipePressure = StartCoroutine(DecreasePipePressure());
    }

    private IEnumerator AccumulateWaste()
    {
        yield return new WaitForSeconds(_accumulateWasteSpeed);
        _wasteTimer -= _accumulateWasteAmount;

        if (_wasteTimer <= 0)
            _gameLost.Raise(this, EventArgs.Empty);
        _wasteTimerChanged.Raise(this, new WasteTimerChangedEventArgs { WasteTimer = _wasteTimer });

        _accumulateWaste = StartCoroutine(AccumulateWaste());
    }

    public void OpenControlPanel(Component sender, object obj)
    {
        _openControlPanel.Raise(this, true);
    }

    public void StartOutputMiniGame(Component sender, object obj)
    {
        if (sender != this) return;
        _canDecreasePower = true;
        _decreaseOutputEfficiency = StartCoroutine(DecreasePowerEfficiency());
    }

    public void StartFanBlockMiniGame(Component sender, object obj)
    {
        if (sender != this) return;

        _canDecreaseFanRPM = true;

        _decreaseFanRPM = StartCoroutine(DecreaseFanRPM());
    }

    public void StartPipePresureMiniGame(Component sender, object obj)
    {
        if (sender != this) return;

        _canDecreasePipePressure = true;

        _decreasePipePressure = StartCoroutine(DecreasePipePressure());
    }

    public void StartWasteManagementMiniGame(Component sender, object obj)
    {
        if (sender != this) return;

        _canAccumulateWaste = true;

        _accumulateWaste = StartCoroutine(AccumulateWaste());
    }

    public void FinishedMiniGame(Component sender, object obj)
    {
        MiniGameFinishedEventArgs args = obj as MiniGameFinishedEventArgs;
        switch (args.FinishedMiniGame) 
        {
            case MiniGame.PowerRegulating:
                StopCoroutine(_decreaseOutputEfficiency);
                _canDecreasePower = false;
                _powerEfficiency = 100;
                _powerEfficiencyChanged.Raise(this, new PowerEfficiencyChangedEventArgs { PowerEfficiency = _powerEfficiency });
                break;
            case MiniGame.FanBlock:
                StopCoroutine(_decreaseFanRPM);
                _canDecreaseFanRPM = false;
                _fanRPM = 3600;
                _fanRPMChanged.Raise(this, new FanRPMChangedEventArgs { FanRPM = _fanRPM });
                break;
            case MiniGame.PiperBroke:
                StopCoroutine(_decreasePipePressure);
                _canDecreasePipePressure = false;
                _pipePSI = 150;
                _pipePressureChanged.Raise(this, new FanRPMChangedEventArgs { FanRPM = _fanRPM });
                break;
        }
    }

    private void Update()
    {
        if (!_closePanel.action.WasPressedThisFrame()) return;
        _openControlPanel.Raise(this, false);
    }
}
