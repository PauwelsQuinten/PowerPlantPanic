using System;
using UnityEngine;

public class MiniGameFinishedEventArgs : EventArgs
{
    public MiniGame FinishedMiniGame;
}

public enum MiniGame 
{
    PowerRegulating,
    FanBlock,
    PiperBroke,
}

