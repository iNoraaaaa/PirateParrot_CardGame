using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Events/Game event (Status)",
    fileName = "GameEventStatus",
    order = 2)]
public class GameEventStatus : ScriptableObject
{
    private readonly List<GameEventStatusListener> listeners = new List<GameEventStatusListener>();
    
    public void Raise(StatusTemplate status, int value)
    {
        for (var i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(status, value);
    }
    
    public void RegisterListener(GameEventStatusListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(GameEventStatusListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
