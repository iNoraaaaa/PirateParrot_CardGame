using UnityEngine;

[CreateAssetMenu(menuName = "CardGame/Effects/ReduceDefense")]
public class ReduceDefenseEffect : Effect
{
    public float reductionPercent;

    public override string GetName()
    {
        return $"Reduce defense by {reductionPercent * 100}%";
    }

    public void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        if (target.Shield != null)
        {
            int reduction = (int)(target.Shield.Value * reductionPercent);
            target.Shield.SetValue(target.Shield.Value - reduction);
        }
    }
}
