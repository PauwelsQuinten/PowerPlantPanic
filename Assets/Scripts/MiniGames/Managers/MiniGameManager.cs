using System;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _miniGameTriggers = new List<GameObject>();
    private IMiniGame _currentMiniGame;

    public void EnableOutputTrigger(Component sender, object obj)
    {
        foreach (GameObject go in _miniGameTriggers)
        {
            if (go.name != "OutputTrigger") continue;
            go.SetActive(!go.activeSelf);
            return;
        }
    }

    public void EnablePressureControlTrigger(Component sender, object obj)
    {
        foreach (GameObject go in _miniGameTriggers)
        {
            if (go.name != "PressureControlTrigger") continue;
            go.SetActive(!go.activeSelf);
            return;
        }
    }

    public void StartMiniGame(Component sender, object obj)
    {
        _currentMiniGame = obj as IMiniGame;
        if (_currentMiniGame == null) return;
        _currentMiniGame.StartMiniGame(this, EventArgs.Empty);
    }
}
