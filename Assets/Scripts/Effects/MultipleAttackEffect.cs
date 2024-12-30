using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Effects/IntegerEffect/Multiple Attack Effect",
    fileName = "MultipleAttackEffect",
    order = 9)]
public class MultipleAttackEffect : IntegerEffect, IEntityEffect
{
    [SerializeField] 
    private int hitCount = 2;

    public override string GetName()
    {
        return $"Deal {Value.ToString()} damage {hitCount} times";
    }

    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        for (int i = 0; i < hitCount; i++)
        {
            var damage = Value;
            if (source.Status != null)
            {
                var powerBoost = source.Status.GetValue("PowerBoost");
                if (powerBoost > 0)
                {
                    damage += powerBoost;
                }
            }

            var targetHp = target.Hp;
            var targetShield = target.Shield;
            
            if (damage >= targetShield.Value)
            {
                var newHp = targetHp.Value - (damage - targetShield.Value);
                if (newHp < 0) newHp = 0;
                targetHp.SetValue(newHp);
                targetShield.SetValue(0);
            }
            else
            {
                targetShield.SetValue(targetShield.Value - damage);
            }
        }
    }
}
