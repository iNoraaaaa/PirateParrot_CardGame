using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人AI控制类用于控制敌人的AI决策，包括随机的效果生成等
/// </summary>
public class EnemyAIManager : BaseManager
{
    [SerializeField]
    private EffectResolutionManager effectResolutionManager;

    [SerializeField] private List<IntentChangeEvent> intentChangeEvents;
        
    private int currentRepeatCount;

    private List<EnemyAI> brains;

    private const float ThinkingTime = 1.5f;


    public override void Initialize(CharacterObject player, List<CharacterObject> enemies)
    {
        base.Initialize(player, enemies);
        brains = new List<EnemyAI>(enemies.Count);

        foreach (var enemy in enemies)
        {
            brains.Add(new EnemyAI(enemy));
        }
    }

    public void OnPlayerTurnBegan()
    {
        const int enemyIndex = 0;

        foreach (var enemy in Enemies)
        {
            var template = enemy.Template as EnemyTemplate;
            var brain = brains[enemyIndex];

            if (template != null)
            {
                if (brain.PatternIndex >= template.Patterns.Count)
                {
                    brain.PatternIndex = 0;
                }

                Sprite sprite = null;
                var pattern = template.Patterns[brain.PatternIndex];

                if (pattern is RepeatPattern repeatPattern)
                {
                    currentRepeatCount += 1;
                    if (currentRepeatCount == repeatPattern.Times)
                    {
                        currentRepeatCount = 0;
                        brain.PatternIndex += 1;
                    }

                    brain.Effects = pattern.Effects;
                    sprite = repeatPattern.Sprite;
                }
                else if (pattern is RandomPattern randomPattern)
                {
                    var effects = new List<int>();
                    var index = 0;
                    
                    // 根据设置的机率，生成对应的效果数量列表，然后对列表进行随机选择
                    foreach (var probability in randomPattern.Probabilities)
                    {
                        var amount = probability.Value;
                        for (var i = 0; i < amount; i++)
                        {
                            effects.Add(index);
                        }

                        index += 1;
                    }

                    var randomIndex = Random.Range(0, effects.Count - 1);
                    var selectedEffect = randomPattern.Effects[effects[randomIndex]];
                    brain.Effects = new List<Effect> { selectedEffect };

                    for (var i = 0; i < pattern.Effects.Count; i++)
                    {
                        var effect = pattern.Effects[i];

                        if (effect == selectedEffect)
                        {
                            sprite = randomPattern.Probabilities[i].Sprite;
                            break;
                        }
                    }
                    
                    brain.PatternIndex += 1;
                }

                var currentEffect = brain.Effects[0];
                if (currentEffect)
                {
                    intentChangeEvents[enemyIndex].Raise(sprite, (currentEffect as IntegerEffect).Value);
                }
            }
        }
    }

    public void OnEnemyTurnBegan()
    {
        StartCoroutine(ProcessEnemyBrains());
    }

    private IEnumerator ProcessEnemyBrains()
    {
        foreach (var brain  in brains)
        {
            effectResolutionManager.SetCurrentEnemy(brain.Enemy);
            effectResolutionManager.ResolveEnemyEffects(brain.Enemy, brain.Effects);
            yield return new WaitForSeconds(ThinkingTime);
        }
    }
    
}
