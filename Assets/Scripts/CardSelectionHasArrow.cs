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
    
    private bool  isArrowCreated = false;
    private AttackArrow _attackArrow;

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

    
    private void DetectCardSelection()
    {
        // 检查玩家是否在卡牌的上方作了点击操作
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var hitInfo = Physics2D.Raycast(mousePosition, Vector3.forward, Mathf.Infinity, cardLayer);

        if (hitInfo.collider != null)
        {
            selectedCard = hitInfo.collider.gameObject;
            selectedCard.GetComponent<SortingGroup>().sortingOrder += 10;
            previousClickPosition = mousePosition;
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
                selectedCard = null;
                isArrowCreated = false;
            });
            
            _attackArrow.EnableArrow(false);
        }
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
}
