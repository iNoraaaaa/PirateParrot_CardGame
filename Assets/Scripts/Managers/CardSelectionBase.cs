using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectionBase : BaseManager
{
    [SerializeField] protected AudioManager audioManager;
    [SerializeField] protected AudioClip cardPlaySound;
    protected Camera mainCamera;
    public LayerMask cardLayer;

    public CardDisplayManager cardDisplayManager;
    public EffectResolutionManager effectResolutionManager;
    public CardDeckManager deckManager;
    
    protected GameObject selectedCard;
    public IntVariable playerMana;

    public LayerMask enemyLayer;

    protected virtual void Start()
    {
        mainCamera = Camera.main;
    }

    protected virtual void PlaySelectedCard()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(cardPlaySound);
        }
        var cardObject = selectedCard.GetComponent<CardObject>();
        var cardTemplate = cardObject.template;
        playerMana.SetValue(playerMana.Value - cardTemplate.Cost);
        
        cardDisplayManager.ReOrganizeHandCards(selectedCard);
        
        // 当卡牌打出后，将手中选中的移除
        cardDisplayManager.MoveCardToDiscardPile(selectedCard);
        deckManager.MoveCardToDiscardPile(cardObject.runtimeCard);
    }
}

