using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeckManager : MonoBehaviour
{
    private List<RuntimeCard> _deck;
    private List<RuntimeCard> _discardPile;
    private List<RuntimeCard> _hand;

    private const int DeckCapacity = 30;
    private const int HandCapacity = 30;
    private const int DiscardPileCapacity = 30;

    public CardDisplayManager cardDisplayManager;

    private DeckWidget _deckWidget;
    private DiscardPileWidget _discardPileWidget;
    
    private void Awake()
    {
        _deck = new List<RuntimeCard>(DeckCapacity);
        _discardPile = new List<RuntimeCard>(DiscardPileCapacity);
        _hand = new List<RuntimeCard>(HandCapacity);
    }

    public void Initialize(DeckWidget deck, DiscardPileWidget discardPile)
    {
        _deckWidget = deck;
        _discardPileWidget = discardPile;
    }

    public int LoadDeck(List<CardTemplate> playerDeck)
    {
        var deckSize = 0;

        foreach (var template in playerDeck)
        {
            if(template == null)
                continue;

            var card = new RuntimeCard
            {
                Template = template
            };
            
            _deck.Add(card);

            ++deckSize;

        }
        
        _deckWidget.SetAmount(_deck.Count);
        _discardPileWidget.SetAmount(0);

        return deckSize;
    }

    public void ShuffleDeck()
    {
        _deck.Shuffle();
    }

    public void DrawCardsFromDeck(int amount)
    {
        var deckSize = _deck.Count;

        if (deckSize >= amount)
        {
            var previousDeckSize = deckSize;

            var drawnCards = new List<RuntimeCard>(amount);

            for (var i = 0; i < amount; i++)
            {
                var card = _deck[0];
                _deck.RemoveAt(0);
                _hand.Add(card);
                drawnCards.Add(card);
            }
            
            cardDisplayManager.CreateHandCards(drawnCards, previousDeckSize);
        }
        // 如果deck中没有足够的牌，则对废弃牌堆中的牌洗一次牌，然后将洗好的牌放于deck中，然后再从deck中
        // 去牌放到手中
        else
        {
            for (var i = 0; i < _discardPile.Count; i++)
            {
                _deck.Add(_discardPile[i]);
            }
            
            _discardPile.Clear();

            cardDisplayManager.UpdateDiscardPileSize(_discardPile.Count);
            
            // 防止抽的牌的数量大于deck的数量
            if (amount > _deck.Count + _discardPile.Count)
            {
                amount = _deck.Count + _discardPile.Count;
            }
            
            DrawCardsFromDeck(amount);
        }
    }

    public void MoveCardToDiscardPile(RuntimeCard card)
    {
        _hand.Remove(card);
        _discardPile.Add(card);
    }

    public void MoveCardsToDiscardPile()
    {
        foreach (var card in _hand)
        {
             _discardPile.Add(card);
        }

        _hand.Clear();
    }
}
