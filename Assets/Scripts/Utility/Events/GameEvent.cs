using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private Dictionary<GameEventListener, int> _listeners = new Dictionary<GameEventListener, int>();

    public void Raise(Component sender = null, object data = null)
    {
        foreach (GameEventListener listener in _listeners.Keys)
        {
            listener.OnEventRaised(sender, data, _listeners[listener]);
        }
    }

    public void RegisterListener(GameEventListener listener, int index)
    {
        bool added = _listeners.TryAdd(listener, index);
        if(!added) 
            Debug.LogError($"Event = {this.name}, listener = {listener.gameObject.name}");
    }
    public void RemoveListener(GameEventListener listener)
    {
        _listeners.Remove(listener);
    }
}
