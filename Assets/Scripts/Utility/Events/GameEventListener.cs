using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomEvent : UnityEvent<Component, object>
{

}

public class GameEventListener : MonoBehaviour
{
    public List<GameEvent> Events = new List<GameEvent>();
    public List<CustomEvent> Responses = new List<CustomEvent>();

    private void OnEnable()
    {
        int i = 0;
        foreach (GameEvent e in Events)
        {
            e.RegisterListener(this, i);
            i++;
        }
    }

    private void OnDisable() 
    { 
        foreach (GameEvent e in Events)
        {
            e.RemoveListener(this);
        }
    }

    private void OnDestroy()
    {
        foreach (GameEvent e in Events)
        {
            e.RemoveListener(this);
        }
    }

    public void OnEventRaised(Component sender, object data, int index)
    {
        if (Responses[index] == null) return;
        Responses[index].Invoke(sender, data);
    }
}
