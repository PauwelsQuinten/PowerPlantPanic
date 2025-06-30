using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerMask;
    [SerializeField]
    private GameEvent _changeInteractionUI;
    [SerializeField]
    private GameEvent _interact;
    [SerializeField]
    private MonoBehaviour _miniGame;


    private bool _isInTrigger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsLayerInMask(_playerMask, collision.gameObject.layer)) return;

        _changeInteractionUI.Raise(this, true);
        _isInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsLayerInMask(_playerMask, collision.gameObject.layer)) return;

        _changeInteractionUI.Raise(this, false);
        _isInTrigger = false;
    }

    private bool IsLayerInMask(LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }

    public void interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!_isInTrigger) return;

        _changeInteractionUI.Raise(this, false);

        if (_miniGame == null)
            _interact.Raise(this, EventArgs.Empty);
        else _interact.Raise(this, _miniGame as IMiniGame);
    }
}
