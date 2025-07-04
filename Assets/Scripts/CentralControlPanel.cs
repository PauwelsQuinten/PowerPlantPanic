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
    private List<GameEvent> _enableMiniGame = new List<GameEvent>();
    [SerializeField]
    private List<GameEvent> _disableMiniGame = new List<GameEvent>();
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

    private List<bool> _isMinigameEnabled = new List<bool>();

    private int _activeMiniGames;

    private int _allowedAmountOfActiveMiniGames = 1;

    private int _completedMinigames;

    private int _maxCompletedMinigames = 5;
    private void Start()
    {
        foreach (GameEvent minigame in _enableMiniGame)
        {
            _isMinigameEnabled.Add(false);
        }

        int index = UnityEngine.Random.Range(0, _enableMiniGame.Count);

        if (_enableMiniGame.Count > 1)
        {
            while (index == _lastEnabledMiniGame)
            {
                index = UnityEngine.Random.Range(0, _enableMiniGame.Count);
            }
        }

        if (index != _lastEnabledMiniGame)
        {
            _isMinigameEnabled[index] = true;
            _enableMiniGame[(int)index].Raise(this, EventArgs.Empty);
            _activeMiniGames += 1;
        }

        _lastEnabledMiniGame = index;

        StartCoroutine(SelectRandomMiniGame());
    }

    private IEnumerator SelectRandomMiniGame()
    {
        yield return new WaitForSeconds(_minigamesInterval);

        if (_activeMiniGames < _allowedAmountOfActiveMiniGames)
        {
            int index = UnityEngine.Random.Range(0, _enableMiniGame.Count);

            if(_enableMiniGame.Count > 1)
            {
                while (index == _lastEnabledMiniGame || _isMinigameEnabled[index])
                {
                    index = UnityEngine.Random.Range(0, _enableMiniGame.Count);
                }
            }

            if(index != _lastEnabledMiniGame)
            {
                _isMinigameEnabled[index] = true;
                _enableMiniGame[index].Raise(this, EventArgs.Empty);
                _activeMiniGames += 1;
            }

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
                _isMinigameEnabled[0] = false;
                _powerEfficiencyChanged.Raise(this, new PowerEfficiencyChangedEventArgs { PowerEfficiency = _powerEfficiency });
                _disableMiniGame[0].Raise(this, EventArgs.Empty);
                break;
            case MiniGame.FanBlock:
                StopCoroutine(_decreaseFanRPM);
                _canDecreaseFanRPM = false;
                _fanRPM = 3600;
                _isMinigameEnabled[2] = false;
                _fanRPMChanged.Raise(this, new FanRPMChangedEventArgs { FanRPM = _fanRPM });
                _disableMiniGame[2].Raise(this, EventArgs.Empty);
                break;
            case MiniGame.PipeBroke:
                StopCoroutine(_decreasePipePressure);
                _canDecreasePipePressure = false;
                _pipePSI = 150;
                _isMinigameEnabled[1] = false;
                _pipePressureChanged.Raise(this, new PipePresureEventArgs { PiperPressure = _pipePSI });
                _disableMiniGame[1].Raise(this, EventArgs.Empty);
                break;
            case MiniGame.WasteManagement:
                StopCoroutine(_accumulateWaste);
                _canAccumulateWaste = false;
                _wasteTimer = 100;
                _isMinigameEnabled[3] = false;
                _wasteTimerChanged.Raise(this, new WasteTimerChangedEventArgs { WasteTimer = _wasteTimer });
                _disableMiniGame[3].Raise(this, EventArgs.Empty);
                break;
        }
        _activeMiniGames -= 1;
        _completedMinigames += 1;

        if(_completedMinigames > _maxCompletedMinigames && _allowedAmountOfActiveMiniGames < 4)
        {
            _allowedAmountOfActiveMiniGames += 1;
            _completedMinigames = 0;
            _maxCompletedMinigames = _maxCompletedMinigames * 2;
        }
    }

    private void Update()
    {
        if (!_closePanel.action.WasPressedThisFrame()) return;
        _openControlPanel.Raise(this, false);
    }
}
