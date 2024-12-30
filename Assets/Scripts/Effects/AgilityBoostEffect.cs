using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Effects/IntegerEffect/Agility Boost Effect",
    fileName = "AgilityBoostEffect",
    order = 11)]
public class AgilityBoostEffect : IntegerEffect, IEntityEffect
{
    [SerializeField]
    private StatusTemplate agilityBoostStatus;

    public override string GetName()
    {
        return $"Gain {Value.ToString()} Defense Boost";
    }

    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        if (target.Status != null && agilityBoostStatus != null)
        {
            var currentAgilityBoost = target.Status.GetValue(agilityBoostStatus.Name);
            target.Status.SetValue(agilityBoostStatus, currentAgilityBoost + Value);
        }
    }
}
