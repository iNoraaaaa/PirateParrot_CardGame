using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Effects/IntegerEffect/Gain Shield Effect",
    fileName = "GainShieldEffect",
    order = 6)]
[Serializable]
public class GainShieldEffect : IntegerEffect, IEntityEffect
{
    public override string GetName()
    {
        return $"Gain {Value.ToString()} Shield ";
    }

    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        var targetShield = target.Shield;
        targetShield.SetValue(targetShield.Value + Value);
    }
}
