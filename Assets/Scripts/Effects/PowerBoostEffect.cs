using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Effects/IntegerEffect/Power Boost Effect",
    fileName = "PowerBoostEffect",
    order = 8)]
public class PowerBoostEffect : IntegerEffect, IEntityEffect
{
    [SerializeField]
    private StatusTemplate powerBoostStatus;

    public override string GetName()
    {
        return $"Gain {Value.ToString()} Power Boost";
    }

    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        if (target.Status != null && powerBoostStatus != null)
        {
            var currentPowerBoost = target.Status.GetValue(powerBoostStatus.Name);
            target.Status.SetValue(powerBoostStatus, currentPowerBoost + Value);
        }
    }
}
