using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(
    menuName = "CardGame/Events/Game event (int)",
    fileName = "GameEventInt",
    order = 1)]
public class GameEventInt : ScriptableObject
{
    private readonly List<GameEventIntListener> listeners = new List<GameEventIntListener>();

    public void Raise(int value)
    {
        for (var i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(value);
        }
    }
    
    public void RegisterListener(GameEventIntListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventIntListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
