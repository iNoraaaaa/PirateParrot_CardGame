using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDriver : MonoBehaviour
{
    public CardBank startingDeck;

    [Header("Manager")]
    [SerializeField] private CardManager cardManager;

    [SerializeField] private CardDisplayManager cardDisplayManager;

    [SerializeField] private CardDeckManager cardDeckManager;

    private List<CardTemplate> _playerDeck = new List<CardTemplate>();
    
    

    private void Start()
    {
        // 清理场景中所有残留的卡牌对象
        var existingCards = FindObjectsOfType<CardObject>();
        foreach (var card in existingCards)
        {
            Destroy(card.gameObject);
        }
        cardManager.Initialize();

        CreatePlayer();
    }

    private void CreatePlayer()
    {
        foreach (var item in startingDeck.Items)
        {
            for (int i = 0; i < item.Amount; i++)
            {
                _playerDeck.Add(item.Card);
            }
        }

        Initialize();
    }

    public void Initialize()
    {
        cardDeckManager.LoadDeck(_playerDeck);
        cardDeckManager.ShuffleDeck();

        cardDisplayManager.Initialize(cardManager);
        
        cardDeckManager.DrawCardsFromDeck(5);
    }
}
