using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventStatusListener : MonoBehaviour
{
    public GameEventStatus Event;
    public UnityEvent<StatusTemplate, int> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(StatusTemplate status, int value)
    {
        Response.Invoke(status, value);
    }
}
