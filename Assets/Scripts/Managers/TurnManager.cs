using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回合管理类
/// 回合管理类是基于时间系统event的，游戏开始时，玩家回合开始，当回合按钮点击事件发生后，敌人回合开始。敌人在施加效果或者攻击后，敌人
/// 回合结束，玩家回合开始
/// </summary>
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
        PlayerTurnBegan.Raise();
        Debug.Log("Begin Player Turn");
    }

    public void EndPlayerTurn()
    {
        PlayerTurnEnded.Raise();
        BeginEnemyTurn();
        Debug.Log("End Player Turn");
    }


    public void BeginEnemyTurn()
    {
        EnemyTurnBegan.Raise();
        isEnemyTurn = true;
        Debug.Log("Begin Enemy Turn");
    }
    
    public void EndEnemyTurn()
    {
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
