using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Effects/IntegerEffect/Multiple Attack Effect",
    fileName = "MultipleAttackEffect",
    order = 9)]
public class MultipleAttackEffect : IntegerEffect, IEntityEffect
{
    [SerializeField] private int attackCount = 2;
    
    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        if (target == null || target.Hp == null)
        {
            Debug.LogError("Target or target HP is null in MultipleAttackEffect");
            return;
        }

        var targetHp = target.Hp;
        var targetShield = target.Shield;
        
        for (int i = 0; i < attackCount; i++)
        {
            var damage = Value;


            if (source.Status != null)
            {
                var weak = source.Status.GetValue("Weak");
                if (weak > 0)
                {
                    damage = (int)Mathf.Floor(damage * 0.75f);
                }

                var powerBoost = source.Status.GetValue("PowerBoost");
                if (powerBoost > 0)
                {
                    damage += powerBoost;
                }
            }

            if (damage >= targetShield.Value)
            {
                var remainingDamage = damage - targetShield.Value;
                targetShield.SetValue(0);
                
                var newHp = Mathf.Max(0, targetHp.Value - remainingDamage);
                targetHp.SetValue(newHp);
            }
            else
            {
                targetShield.SetValue(targetShield.Value - damage);
            }
            
            Debug.Log($"Multiple Attack: Deal {damage} damage (Attack {i + 1}/{attackCount})");
        }
    }

    public override string GetName()
    {
        return $"Deal {Value} damage {attackCount} times";
    }
}
