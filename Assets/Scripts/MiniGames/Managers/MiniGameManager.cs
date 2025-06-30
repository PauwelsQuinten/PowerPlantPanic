using System;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    private IMiniGame _currentMiniGame;

    public void StartMiniGame(Component sender, object obj)
    {
        _currentMiniGame = obj as IMiniGame;
        if (_currentMiniGame == null) return;
        _currentMiniGame.StartMiniGame(this, EventArgs.Empty);
    }
}
