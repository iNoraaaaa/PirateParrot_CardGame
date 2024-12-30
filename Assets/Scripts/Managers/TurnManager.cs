using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameEvent PlayerTurnBegan;
    public GameEvent PlayerTurnEnded;
    public GameEvent EnemyTurnBegan;
    public GameEvent EnemyTurnEnded;

    private bool isEnemyTurn;
    private float timer;
    private bool isEndOfGame;

    private const float EnemyTurnDuration = 3.0f;

    private void Update()
    {
        if (isEnemyTurn)
        {
            timer += Time.deltaTime;

            if (timer >= EnemyTurnDuration)
            {
                timer = 0.0f;
                EndEnemyTurn();
                BeginPlayerTurn();
            }
        }
    }

    public void BeginGame()
    {
        BeginPlayerTurn();
    }

    public void BeginPlayerTurn()
    {
        if (isEndOfGame) return;
        PlayerTurnBegan.Raise();
        Debug.Log("Begin Player Turn");
    }

    public void EndPlayerTurn()
    {
        if (isEndOfGame) return;
        PlayerTurnEnded.Raise();
        BeginEnemyTurn();
        Debug.Log("End Player Turn");
    }

    public void BeginEnemyTurn()
    {
        if (isEndOfGame) return;
        EnemyTurnBegan.Raise();
        isEnemyTurn = true;
        Debug.Log("Begin Enemy Turn");
    }
    
    public void EndEnemyTurn()
    {
        if (isEndOfGame) return;
        EnemyTurnEnded.Raise();
        isEnemyTurn = false;
        Debug.Log("End Enemy Turn");
    }
    
    public void SetEndOfGame(bool value)
    {
        isEndOfGame = value;
        Debug.Log("End of Game");
    }

    public bool IsEndOfGame()
    {
        return isEndOfGame;
    }
}
