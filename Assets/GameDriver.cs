using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDriver : MonoBehaviour
{
    public CardBank startingDeck;
    
    [Header("Manager")]
    [SerializeField] private CardManager cardManager;
    [SerializeField] private CardDeckManager cardDeckManager;

    private List<CardTemplate> _playerDeck = new List<CardTemplate>();

    private void Start()
    {
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
    }
    
    public void Initialize()
    {
        cardDeckManager.LoadDeck(_playerDeck);
        cardDeckManager.ShuffleDeck();
        cardDeckManager.DrawCardsFromDeck(5);
    }
}
