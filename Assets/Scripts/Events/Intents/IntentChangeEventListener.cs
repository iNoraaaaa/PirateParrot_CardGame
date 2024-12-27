using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntentChangeEventListener : MonoBehaviour
{
    public IntentChangeEvent Event;
    public UnityEvent<Sprite, int> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Sprite sprite, int value)
    {
        Response.Invoke(sprite, value);
    }
}