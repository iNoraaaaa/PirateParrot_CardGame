using UnityEngine;
using UnityEngine.Events;
using System;

[CreateAssetMenu(
    menuName = "CardGame/Variables/Integer",
    fileName = "IntegerVariable",
    order = 0)]
public class IntVariable : ScriptableObject
{
    [SerializeField]
    private int _value;
    
    public int Value 
    {
        get => _value;
        set 
        {
            _value = Mathf.Max(0, value);
            OnValueChanged?.Invoke(_value);
            ValueChangedEvent?.Raise(_value);
        }
    }
    
    public GameEventInt ValueChangedEvent;
    public event Action<int> OnValueChanged;

    private void OnEnable()
    {
        // 确保字段初始化
        if (ValueChangedEvent == null)
        {
            ValueChangedEvent = CreateInstance<GameEventInt>();
        }
        _value = 0;
    }

    public void SetValue(int value)
    {
        Value = value;
    }
}
