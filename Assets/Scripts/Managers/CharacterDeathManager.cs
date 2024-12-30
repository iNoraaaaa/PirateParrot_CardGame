using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeathManager : BaseManager
{
    [SerializeField] private float endBattlePopupDelay = 1.0f;
    [SerializeField] private EndBattlePopup endBattlePopup;
    [SerializeField] private GameDriver gameDriver;

    public void OnPlayerHpChanged(int hp)
    {
        if (hp <= 0)
        {
            EndGame(true);
        }
    }

    public void OnEnemyHpChanged(int hp)
    {
        if (hp <= 0)
        {
            Enemies[0].OnCharacterDied();
            EndGame(false);
        }
    }

    public void EndGame(bool characterDied)
    {
        StartCoroutine(ShowEndBattlePopup(characterDied));
    }

    private IEnumerator ShowEndBattlePopup(bool characterDied)
    {
        yield return new WaitForSeconds(endBattlePopupDelay);

        if (endBattlePopup != null)
        {
            endBattlePopup.Show();

            if (characterDied)
            {
                endBattlePopup.SetDefeatText();
            }
            else
            {
                endBattlePopup.SetVictoryText();
            }

            var turnManagement = FindFirstObjectByType<TurnManager>();
            turnManagement.SetEndOfGame(true);
        }
    }
    
    public override void Initialize(CharacterObject player, List<CharacterObject> enemies)
    {
        base.Initialize(player, enemies);
        
        if(gameDriver == null)
        {
            gameDriver = FindObjectOfType<GameDriver>();
            if(gameDriver == null)
            {
                Debug.LogError("GameDriver reference not found!");
                return;
            }
        }
        
        foreach (var enemy in enemies)
        {
            if(enemy != null && enemy.Character != null && enemy.Character.Hp != null)
            {
                enemy.Character.Hp.OnValueChanged += value => 
                {
                    if (value <= 0)
                    {
                        enemy.OnCharacterDied();
                        if(gameDriver != null)
                        {
                            gameDriver.OnEnemyDefeated();
                        }
                    }
                };
            }
            else
            {
                Debug.LogError("Enemy or its components are null!");
            }
        }

        if(player != null && player.Character != null && player.Character.Hp != null)
        {
            player.Character.Hp.OnValueChanged += value =>
            {
                if (value <= 0)
                {
                    player.OnCharacterDied();
                    EndGame(true);
                }
            };
        }
        else
        {
            Debug.LogError("Player or its components are null!");
        }
    }
}
