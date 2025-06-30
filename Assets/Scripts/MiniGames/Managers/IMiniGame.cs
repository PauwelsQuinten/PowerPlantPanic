using System;
using UnityEngine;

public interface IMiniGame
{
    public void StartMiniGame(Component sender, object obj);

    public void completed();

    public void failed();
}
