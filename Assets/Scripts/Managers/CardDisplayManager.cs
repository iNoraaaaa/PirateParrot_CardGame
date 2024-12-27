using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class CardDisplayManager : MonoBehaviour
{
    private const int PositionNumber = 20;
    private const int RotationNumber = 20;
    private const int SortingOrdersNumber = 20;

    private CardManager _cardManager;
    private DeckWidget _deckWidget;
    private DiscardPileWidget _discardPileWidget;

    private List<Vector3> _positions;
    private List<Quaternion> _rotations;
    private List<int> _sortingOrder;

    private readonly List<GameObject> _handCards = new (PositionNumber);

    private const float Radius = 16.0f;

    private readonly Vector3 _center = new(-15.0f, -18.5f, 0.0f);
    private readonly Vector3 _originalCardScale = new(0.5f, 0.5f, 1.0f);

    private bool isCardMoving;

    public static float CardToDiscardPileAnimationTime = 0.3f;

    public void Initialize(CardManager cardManager, DeckWidget deck, DiscardPileWidget discardPileWidget)
    {
        _cardManager = cardManager;
        _deckWidget = deck;
        _discardPileWidget = discardPileWidget;
    }

    private void Start()
    {
        _positions = new(PositionNumber);
        _rotations = new(RotationNumber);
        _sortingOrder = new(SortingOrdersNumber);
    }

    public void CreateHandCards(List<RuntimeCard> cardsInHand, int deckSize)
    {
        var drawnCards = new List<GameObject>(cardsInHand.Count);

        foreach (var card in cardsInHand)
        {
            var cardGameObject = CreateCardGameObject(card);
            _handCards.Add(cardGameObject);
            drawnCards.Add(cardGameObject);
        }
        
        _deckWidget.SetAmount(deckSize);
        
        PutDeckCardsToHand(drawnCards);
    }

    private GameObject CreateCardGameObject(RuntimeCard card)
    {
        var gameObj = _cardManager.GetObject();
        var cardObject = gameObj.GetComponent<CardObject>();
        cardObject.SetInfo(card);

        gameObj.transform.position = Vector3.zero;
        gameObj.transform.localScale = Vector3.zero;

        return gameObj;
    }

    private void PutDeckCardsToHand(List<GameObject> drawnCards)
    {
        isCardMoving = true;
        
        OrganizeHandCards();

        var interval = 0.0f;

        for (var i = 0; i < _handCards.Count; i++)
        {
            var j = i;

            const float time = 0.5f;

            var card = _handCards[i];

            if (drawnCards.Contains(card))
            {
                var cardObject = card.GetComponent<CardObject>();

                var seq = DOTween.Sequence();
                seq.AppendInterval(interval);
                seq.AppendCallback(() =>
                {
                    _deckWidget.RemoveCard();
                    var move = card.transform.DOMove(_positions[j], time).OnComplete(() =>
                    {
                        cardObject.SaveTransform(_positions[j], _rotations[j]);
                    });

                    card.transform.DORotateQuaternion(_rotations[j], time);
                    card.transform.DOScale(_originalCardScale, time);

                    if (j == _handCards.Count - 1)
                    {
                        move.OnComplete(() =>
                        {
                            isCardMoving = false;
                            cardObject.SaveTransform(_positions[j], _rotations[j]);
                        });
                    }
                });
            }

            card.GetComponent<SortingGroup>().sortingOrder = _sortingOrder[i];

            interval += 0.2f;
        }
        
        
    }

    private void OrganizeHandCards()
    {
        _positions.Clear();
        _rotations.Clear();
        _sortingOrder.Clear();

        const float angle = 5.0f;
        var cardAngle = (_handCards.Count - 1) * angle / 2;
        var z = 0.0f;

        for (var i = 0; i < _handCards.Count; ++i)
        {
            // Rotate
            var rotation = Quaternion.Euler(0, 0, cardAngle - i * angle);
            _rotations.Add(rotation);
            
            // Move
            z -= 0.1f;
            var position = CalculateCardPosition(cardAngle - i * angle);
            position.z = z;
            _positions.Add(position);

            _sortingOrder.Add(i);
        }
    }

    private Vector3 CalculateCardPosition(float angle)
    {
        return new Vector3(
            _center.x - Radius * Mathf.Sin(Mathf.Deg2Rad * angle),
            _center.y + Radius * Mathf.Cos(Mathf.Deg2Rad * angle),
            0.0f);
    }

    public bool isMoving()
    {
        return isCardMoving;
    }

    public void ReOrganizeHandCards(GameObject selectedCard)
    {
        _handCards.Remove(selectedCard);
        
        // 卡牌位置的重新调整
        OrganizeHandCards();
        
        // 以动画的方式动态调整卡牌图形        
        for (var i = 0; i < _handCards.Count; i++)
        {
            var card = _handCards[i];
            const float time = 0.3f;
            card.transform.DOMove(_positions[i], time);
            card.transform.DORotateQuaternion(_rotations[i], time);
            card.GetComponent<SortingGroup>().sortingOrder = _sortingOrder[i];
            card.GetComponent<CardObject>().SaveTransform(_positions[i], _rotations[i]);
        }
    }

    public void MoveHandToDiscardPile()
    {
        foreach (var card in _handCards)
        {
            MoveCardToDiscardPile(card);
        }
        _handCards.Clear();
        
    }
    

    public void MoveCardToDiscardPile(GameObject gameObj)
    {
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            gameObj.transform.DOScale(Vector3.zero, CardToDiscardPileAnimationTime).OnComplete(() =>
            {
                gameObj.GetComponent<CardManager.ManagedPoolObject>().cardManager.ReturnObject(gameObj);
            });
        });
        sequence.AppendCallback(() =>
        {
            _discardPileWidget.AddCard();
            _handCards.Remove(gameObj);
        });
    }

    public void UpdateDiscardPileSize(int size)
    {
        _discardPileWidget.SetAmount(size);
    }
}
