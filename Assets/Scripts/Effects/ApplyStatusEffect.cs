using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(
    menuName = "CardGame/Effects/IntegerEffect/Apply Status Effect",
    fileName = "ApplyStatusEffect",
    order = 7)]
public class ApplyStatusEffect : IntegerEffect, IEntityEffect
{
    public StatusTemplate Status;
    
    public override string GetName()
    {
        if (Status != null)
        {
            return $"Apply {Value.ToString()} {Status.Name}";
        }

        return "Apply status";
    }

    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        var currentValue = target.Status.GetValue(Status.Name);
        target.Status.SetValue(Status, currentValue + Value);
    }
}
