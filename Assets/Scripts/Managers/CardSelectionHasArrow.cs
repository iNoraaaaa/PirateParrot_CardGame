using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class CardSelectionHasArrow : CardSelectionBase
{
    private Vector3 previousClickPosition;

    private const float CardDetectionOffset = 2.2f;
    private const float CardAnimationTime = 0.2f;

    private const float SelectedCardYOffset = -1.0f;
    private const float AttackCardInMiddlePositionX = -15;

    private AttackArrow _attackArrow;
    private bool isArrowCreated;
    private GameObject _selectedEnemy;
    
    

    protected override void Start()
    {
        base.Start();
        _attackArrow = FindFirstObjectByType<AttackArrow>();
    }

    
    private void Update()
    {
        if (cardDisplayManager.isMoving())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            DetectCardSelection();
            DetectEnemySelection();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            DetectEnemySelection();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            DetectCardUnselection();
        }

        if (selectedCard != null)
        {
            UpdateCardAndTargetingArrow();
        }
    }


    private void DetectEnemySelection()
    {
        if (selectedCard != null)
        {
            // 检查鼠标是否选中了一个敌人
            var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hitInfo = Physics2D.Raycast(mousePosition, Vector3.forward, Mathf.Infinity, enemyLayer);
            if (hitInfo.collider != null)
            {
                _selectedEnemy = hitInfo.collider.gameObject;
                PlaySelectedCard();

                selectedCard = null;
                
                // 这是鼠标左键按下，这时需要取消攻击显示攻击箭头，为后续攻击效果的显示做准备
                isArrowCreated = false;
                _attackArrow.EnableArrow(false);
            }
        }
    }
    
    private void DetectCardSelection()
    {
        // 检查玩家是否在卡牌的上方作了点击操作
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var hitInfo = Physics2D.Raycast(mousePosition, Vector3.forward, Mathf.Infinity, cardLayer);

        if (hitInfo.collider != null)
        {
            var card = hitInfo.collider.GetComponent<CardObject>();
            var cardTemplate = card.template;

            if (CardUtils.CardCanBePlayed(cardTemplate, playerMana) && 
                CardUtils.CardHasTargetableEffect(cardTemplate))
            {
                selectedCard = hitInfo.collider.gameObject;
                selectedCard.GetComponent<SortingGroup>().sortingOrder += 10;
                previousClickPosition = mousePosition;
            }
        }
    }

    private void DetectCardUnselection()
    {
        if (selectedCard != null)
        {
            var card = selectedCard.GetComponent<CardObject>();
            selectedCard.transform.DOKill();
            card.Reset(() =>
            {
                isArrowCreated = false;
                selectedCard = null;
            });
            
            _attackArrow.EnableArrow(false);
        }
    }

    protected override void PlaySelectedCard()
    {
        base.PlaySelectedCard();
        var card = selectedCard.GetComponent<CardObject>().runtimeCard;
        effectResolutionManager.ResolveCardEffects(card, _selectedEnemy.GetComponent<CharacterObject>());
    }
    
    private void UpdateCardAndTargetingArrow()
    {
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var diffY = mousePosition.y - previousClickPosition.y;

        if (!isArrowCreated && diffY > CardDetectionOffset)
        {
            isArrowCreated = true;
            
            var position = selectedCard.transform.position;

            selectedCard.transform.DOKill();

            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() =>
            {
                selectedCard.transform.DOMove(new Vector3(AttackCardInMiddlePositionX, SelectedCardYOffset, position.z),
                    CardAnimationTime);

                selectedCard.transform.DORotate(Vector3.zero, CardAnimationTime);
            });

            sequence.AppendInterval(0.15f);
            sequence.OnComplete(() =>
            {
                _attackArrow.EnableArrow(true);
            });
        }
    }
    
    
    public GameObject GetSelectedEnemy()
    {
        return _selectedEnemy;
    }

    public bool HasSelectedCard()
    {
        return selectedCard != null;
    }
}
