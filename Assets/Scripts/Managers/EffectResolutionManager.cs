using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectResolutionManager : BaseManager
{
    public CardSelectionHasArrow cardSelectionHasArrow;
    private CharacterObject _currentEnemy;

    public void ResolveCardEffects(RuntimeCard card, CharacterObject playerSelectedTarget)
    {
        if (card == null || card.Template == null || card.Template.Effects == null)
        {
            Debug.LogError("Invalid card or effects in ResolveCardEffects");
            return;
        }

        foreach (var effect in card.Template.Effects)
        {
            if (effect == null) continue;

            if (effect is TargetableEffect targetableEffect)
            {
                if (playerSelectedTarget == null)
                {
                    Debug.LogError("No target selected for targetable effect");
                    continue;
                }

                var targets = GetTargets(targetableEffect, playerSelectedTarget, true);
                if (targets != null && targets.Count > 0)
                {
                    foreach (var target in targets)
                    {
                        targetableEffect.Resolve(Player.Character, target.Character);
                    }
                }
            }
            else if (effect is IEntityEffect entityEffect)
            {
                entityEffect.Resolve(Player.Character, playerSelectedTarget.Character);
            }
        }
    }

    public void ResolveCardEffects(RuntimeCard card)
    {
        foreach (var effect in card.Template.Effects)
        {
            var targetableEffect = effect as TargetableEffect;

            if (targetableEffect != null)
            {
                var targets = GetTargets(targetableEffect, null, true);

                foreach (var target in targets)
                {
                    targetableEffect.Resolve(Player.Character, target.Character);
                }
            }
        }
    }
    
    public void SetCurrentEnemy(CharacterObject enemy)
    {
        _currentEnemy = enemy;
    }

    public void ResolveEnemyEffects(CharacterObject enemy, List<Effect> effects)
    {
        foreach (var effect in effects)
        {
            var targetableEffect = effect as TargetableEffect;
            if (targetableEffect != null)
            {
                var targets = GetTargets(targetableEffect, null, false);

                foreach (var target in targets)
                {
                    targetableEffect.Resolve(enemy.Character, target.Character);
                }
            }
        }
    }
    

    private List<CharacterObject> GetTargets(
        TargetableEffect effect,
        CharacterObject playerSelectedTarget,
        bool playerSource)
    {
        var targets = new List<CharacterObject>(4);
        
        // 如果动作发起方是主角Player
        if (playerSource)
        {
            switch (effect.Target)
            {
                case EffectTargetType.Self:
                    targets.Add(Player);
                    break;
                case EffectTargetType.TargetEnemy:
                    targets.Add(playerSelectedTarget);
                    break;
            }
        }
        // 如果动作发起方是敌人
        else
        {
            switch (effect.Target)
            {
                case EffectTargetType.Self:
                    targets.Add(_currentEnemy);
                    break;
                case EffectTargetType.TargetEnemy:
                    targets.Add(Player);
                    break;
            }
        }

        return targets;
    }

   
    
}
