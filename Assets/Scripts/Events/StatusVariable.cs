using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Variables/Status Variable",
    fileName = "StatusVariable",
    order = 1)]
public class StatusVariable : ScriptableObject
{
    public Dictionary<string, int> Value = new Dictionary<string, int>();
    public Dictionary<string, StatusTemplate> Template = new Dictionary<string, StatusTemplate>();

    public GameEventStatus ValueChangedEvent;

    public int GetValue(string status)
    {
        if (Value.ContainsKey(status))
        {
            return Value[status];
        }

        return 0;
    }

    public void SetValue(StatusTemplate statusTemplate, int value)
    {
        var statusName = statusTemplate.Name;
        Value[statusName] = value;
        ValueChangedEvent?.Raise(statusTemplate, value);

        if (!Template.ContainsKey(statusName))
        {
            Template.Add(statusName, statusTemplate);
        }
    }
}
