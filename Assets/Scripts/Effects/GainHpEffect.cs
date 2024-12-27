using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Effects/IntegerEffect/Gain Hp Effect",
    fileName = "Gain Hp Effect",
    order = 5)]
public class GainHpEffect : IntegerEffect, IEntityEffect
{
    public override string GetName()
    {
        return $"Gain {Value.ToString()}  HP";
    }

    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        var targetHp = target.Hp;
        var finalHp = targetHp.Value + Value;

        if (finalHp > target.MaxHp)
        {
            finalHp = target.MaxHp;
        }
        
        targetHp.SetValue(finalHp);
        Debug.Log("Gain Hp");
    }
}
