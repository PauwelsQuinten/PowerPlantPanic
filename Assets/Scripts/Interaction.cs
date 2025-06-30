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


    private bool _isInTrigger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsLayerInMask(_playerMask, collision.gameObject.layer)) return;

        _changeInteractionUI.Raise(this, EventArgs.Empty);
        _isInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsLayerInMask(_playerMask, collision.gameObject.layer)) return;

        _changeInteractionUI.Raise(this, EventArgs.Empty);
        _isInTrigger = false;
    }

    private bool IsLayerInMask(LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }

    public void interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed && !_isInTrigger) return;

        _interact.Raise(this, EventArgs.Empty);
    }
}
