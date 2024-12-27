using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventIntListener : MonoBehaviour
{
    // 所侦听的事件
    public GameEventInt Event;
    
    // 事件被激发时的响应
    public UnityEvent<int> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(int value)
    {
        Response.Invoke(value);
    }
    
}
