using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Events/Game event (Intent change)",
    fileName = "GameEventIntentChange",
    order = 3)]
public class IntentChangeEvent : ScriptableObject
{
    private readonly List<IntentChangeEventListener> listeners = new List<IntentChangeEventListener>();

    public void Raise(Sprite sprite, int value)
    {
        for (var i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(sprite, value);
    }

    public void RegisterListener(IntentChangeEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(IntentChangeEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}