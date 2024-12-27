using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeathManager : BaseManager
{
    [SerializeField] private float endBattlePopupDelay = 1.0f;
    [SerializeField] private EndBattlePopup endBattlePopup;
    
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
        // yield return new WaitForSeconds(0.2f);
        // Debug.Log("Game End");
        
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
    
    
}
