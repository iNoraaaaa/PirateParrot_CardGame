using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Enemy/Priest Pattern",
    fileName = "PriestPattern",
    order = 0)]
public class PriestPattern : Pattern
{
    public enum PriestActionType
    {
        Purification,    // 净化惩戒
        HolyRecovery,   // 神圣恢复
        PunishingLight  // 惩戒之光
    }
    
    [SerializeField] private PriestActionType actionType;
    [SerializeField] private int damage;
    [SerializeField] private int healing;
    [SerializeField] private float defenseReductionPercent;

    [SerializeField] private DealDamageEffect dealDamageEffect;
    [SerializeField] private GainHpEffect gainHpEffect;
    [SerializeField] private GainShieldEffect gainShieldEffect;

    public override string GetName()
    {
        switch (actionType)
        {
            case PriestActionType.Purification:
                return $"Deal {damage} damage and apply Purification";
            case PriestActionType.HolyRecovery:
                return $"Heal {healing} HP";
            case PriestActionType.PunishingLight:
                return $"Deal {damage} damage and reduce defense";
            default:
                return "Unknown action";
        }
    }

    private void OnEnable()
    {
        Effects.Clear();
        switch (actionType)
        {
            case PriestActionType.Purification:
                if (dealDamageEffect != null)
                {
                    dealDamageEffect.Value = damage;
                    Effects.Add(dealDamageEffect);
                }
                break;
                
            case PriestActionType.HolyRecovery:
                if (gainHpEffect != null)
                {
                    gainHpEffect.Value = healing;
                    Effects.Add(gainHpEffect);
                }
                break;
                
            case PriestActionType.PunishingLight:
                if (dealDamageEffect != null)
                {
                    dealDamageEffect.Value = damage;
                    Effects.Add(dealDamageEffect);
                }
                break;
        }
    }
}
