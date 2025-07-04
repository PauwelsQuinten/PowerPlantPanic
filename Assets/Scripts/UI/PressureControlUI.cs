using System.Collections.Generic;
using UnityEngine;

public class PressureControlUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _ui;
    [SerializeField]
    private GameEvent _ChangeCanMove;

    public void EnableUi(Component sender, object obj)
    {
        bool? setActive = obj as bool?;
        if ((bool)setActive)
        {
            _ui.SetActive(true);
            _ChangeCanMove.Raise(this, false);
        }
        else
        {
            _ui.SetActive(false);
            _ChangeCanMove.Raise(this, true);
        }
    }

    public void ValveRotationChanged(Component sender, object obj)
    {
        ValveRotationChangedEventArgs args = obj as ValveRotationChangedEventArgs;

        if (args == null) return;

        args.Valve.transform.eulerAngles = new Vector3(0, 0, args.ValveRotation * -1);
    }
}
