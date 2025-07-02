using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PresureRegulator : MonoBehaviour, IMiniGame
{
    [SerializeField]
    private List<GameObject> _pipes = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _pipePrefabs = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _brokenPipePrefabs = new List<GameObject>();
    [SerializeField]
    private GameEvent _ValveRotationChanged;
    [SerializeField]
    private GameEvent _openPressureControlUI;
    [SerializeField]
    private GameObject _itemHolder;
    [SerializeField]
    private InputActionReference _closePanel;
    [SerializeField]
    private GameEvent _miniGameFinished;


    private GameObject _brokenPipe;
    private int _currentBrokenPipeIndex;
    private Coroutine _checkForMouseInput;

    private GameObject _activeValve;
    private GameObject _heldItem;

    private float _valveProgress = 0;

    private bool _valveIsOpen = true;

    private bool _isCarryingPipe = false;

    private bool _itemPlaced = false;

    private bool _valveLocked = false;

    private bool _miniGameStarted = false;

    private void SetRandomBrokenPipe()
    {
        int randomPipe = UnityEngine.Random.Range(0, _pipes.Count);

        _currentBrokenPipeIndex = randomPipe;

        for (int i = 0; i < _brokenPipePrefabs.Count; i++)
        {
            if (_brokenPipePrefabs[i].tag != _pipes[randomPipe].tag) continue;
            GameObject brokenPipe = Instantiate(_brokenPipePrefabs[i], _pipes[randomPipe].transform.position, _pipes[randomPipe].transform.rotation);
            _brokenPipe = brokenPipe;
        }

        Destroy(_pipes[randomPipe]);
    }

    public void StartMiniGame(Component sender, object obj)
    {
        if (_miniGameStarted) return;
        _miniGameStarted = true;
        SetRandomBrokenPipe();
    }

    public void RemovePipe(Component sender, object obj)
    {
        string pipeHolderTag = sender.gameObject.transform.parent.tag;
        if (_isCarryingPipe)
        {
            PlacePipe(pipeHolderTag, sender.transform.parent.gameObject);
            return;
        }

        if (pipeHolderTag != _brokenPipe.tag) return;
        if (_valveIsOpen) return;
        _isCarryingPipe = true;
        _heldItem = _brokenPipe;
        _brokenPipe.transform.parent = _itemHolder.transform;
        _brokenPipe.transform.localPosition = Vector3.zero;
        _brokenPipe.transform.localEulerAngles = new Vector3(0, 0, 90);
    }

    private void PlacePipe(string tag, GameObject holder)
    {
        if (_heldItem.tag != tag) return;
        if (_brokenPipe != null) return;

        _heldItem.transform.parent = holder.transform;
        _heldItem.transform.localPosition = Vector3.zero;
        _heldItem.transform.localEulerAngles = new Vector3(0, 0, 0);

        _heldItem = null;
        _pipes.Insert(_currentBrokenPipeIndex, _heldItem);
        _itemPlaced = true;
    }

    public void GrabItem(Component sender, object obj)
    {
        if (_heldItem != null) return;
        string pipeHolderTag = sender.gameObject.transform.parent.tag;

        for(int i = 0; i < _pipePrefabs.Count; i++)
        {
            if (_pipePrefabs[i].tag != pipeHolderTag) continue;
            GameObject go = Instantiate(_pipePrefabs[i]);
            _heldItem = go;
            go.transform.parent = _itemHolder.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
    }

    public void TrashItem(Component sender, object obj)
    {
        if (_heldItem == null) return;

        Destroy(_heldItem.gameObject);
    }

    public void completed()
    {
        _miniGameStarted = false;
        _miniGameFinished.Raise(this, new MiniGameFinishedEventArgs{ FinishedMiniGame = MiniGame.PipeBroke});
    }

    public void failed()
    {
        _miniGameStarted = false;
    }

    public void GetActiveValve(Component sender, object obj)
    {
        _activeValve = obj as GameObject;
    }

    public void OpenPressureControl(Component sender, object obj)
    {
        if (_heldItem != null) return;
        _openPressureControlUI.Raise(this, true);
        StartMiniGame(sender, obj);
    }

    private void Update()
    {
        if (_closePanel.action.WasPressedThisFrame())
        {
            _openPressureControlUI.Raise(this, false);
        }

        if (!_miniGameStarted) return;
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _valveLocked = false;
        }
        if (!Mouse.current.leftButton.IsPressed()) return;
        if (_activeValve == null) return;

        switch (_currentBrokenPipeIndex)
        {
            case 0:
                if (_activeValve.tag != "Red") break;
                if (_valveLocked) break;
                if (_valveIsOpen) _valveProgress += 40 * Time.deltaTime;
                else _valveProgress -= 40 * Time.deltaTime;
                break;
            case 1:
                if (_activeValve.tag != "Green") break;
                if (_valveLocked) break;
                if (_valveIsOpen) _valveProgress += 40 * Time.deltaTime;
                else _valveProgress -= 40 * Time.deltaTime;
                break;
            case 2:
                if (_activeValve.tag != "Blue") break;
                if (_valveLocked) break;
                if (_valveIsOpen) _valveProgress += 40 * Time.deltaTime;
                else _valveProgress -= 40 * Time.deltaTime;
                break;
        }

        if (_valveProgress > 180)
        {
            _valveIsOpen = false;
            _valveLocked = true;
        }


        if (_valveProgress < 0)
        {
            _valveIsOpen = true;
            _valveLocked = true;

            completed();
        }

        _ValveRotationChanged.Raise(this, new ValveRotationChangedEventArgs { ValveRotation = _valveProgress, Valve = _activeValve });
    }
}
