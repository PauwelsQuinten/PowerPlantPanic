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
    private InputActionReference _closePanel;
    [SerializeField]
    private GameEvent _gameLost;

    private int _lastEnabledMiniGame = -1;

    private int _powerEfficiency = 100;

    private Coroutine _decreaseOutputEfficiency;

    private bool _canDecreasePower = false;


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
            yield return new WaitForSeconds(0.5f);

            _powerEfficiency -= 1;
            if (_powerEfficiency <= 0) 
                _gameLost.Raise(this, EventArgs.Empty);
            _powerEfficiencyChanged.Raise(this, new PowerEfficiencyChangedEventArgs { PowerEfficiency = _powerEfficiency });
            _decreaseOutputEfficiency = StartCoroutine(DecreasePowerEfficiency());
        }
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
        }
    }

    private void Update()
    {
        if (!_closePanel.action.WasPressedThisFrame()) return;
        _openControlPanel.Raise(this, false);
    }
}
