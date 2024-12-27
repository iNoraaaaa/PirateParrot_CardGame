using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Effects/IntegerEffect/Deal Damage Effect",
    fileName = "DealDamageEffect",
    order = 4)]
public class DealDamageEffect : IntegerEffect, IEntityEffect
{
    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        var targetHp = target.Hp;
        var hp = targetHp.Value;

        var targetShield = target.Shield;
        var shield = targetShield.Value;
        
        var damage = Value;

        if (source.Status != null)
        {
            var weak = source.Status.GetValue("Weak");

            if (weak > 0)
            {
                damage = (int)Mathf.Floor(damage * 0.75f);
            }
        }

        if (damage >= shield)
        {
            var newHp = hp - (damage - shield);
            if (newHp < 0)
            {
                newHp = 0;
            }
            targetHp.SetValue(newHp);
            targetShield.SetValue(0);
        }
        else
        {
            targetShield.SetValue(shield - damage);
        }
        
        Debug.Log("Deal Damage" + damage);

        // var newHp = hp - damage;
        // if (newHp < 0)
        // {
        //     newHp = 0;
        // }
        //
        // targetHp.SetValue(newHp);
    }

    public override string GetName()
    {
        return $"Deal {Value.ToString()} damage";
    }
}
