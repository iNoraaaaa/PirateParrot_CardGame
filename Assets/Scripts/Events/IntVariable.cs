using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Variables/Integer",
    fileName = "IntegerVariable",
    order = 0)]
public class IntVariable : ScriptableObject
{
    public int Value;

    public GameEventInt ValueChangedEvent;

    public void SetValue(int value)
    {
        Value = value;
        Debug.Log("hp = " + value);
        ValueChangedEvent?.Raise(value);
    }
}
