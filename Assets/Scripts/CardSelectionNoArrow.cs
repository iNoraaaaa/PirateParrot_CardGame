using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class CardSelectionNoArrow : CardSelectionBase
{
    private Vector3 originalCardPosition;
    private Quaternion originalCardRotation;
    private int originalCardSortingOrder;

    private const float cardCancelAnimationTime = 0.2f;
    private const Ease cardAnimationEase = Ease.OutBack;

    private void Update()
    {
        if (cardDisplayManager.isMoving())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            DetectCardSelection();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DetectCardUnselection();
        }

        if (selectedCard != null)
        {
            UpdateSelectedCard();
        }
    }

   

    private void DetectCardSelection()
    {
        if (selectedCard != null)
            return;
        
        // 检查玩家是否在卡牌的上方作了点击操作
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var hitInfo = Physics2D.Raycast(mousePosition, Vector3.forward,
            Mathf.Infinity, cardLayer);

        if (hitInfo.collider != null)
        {
            selectedCard = hitInfo.collider.gameObject;
            originalCardPosition = selectedCard.transform.position;
            originalCardRotation = selectedCard.transform.rotation;
            originalCardSortingOrder = selectedCard.GetComponent<SortingGroup>().sortingOrder;
        }
    }
    
    private void DetectCardUnselection()
    {
        if (selectedCard != null)
        {
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() =>
            {
                selectedCard.transform.DOMove(originalCardPosition, cardCancelAnimationTime).SetEase(cardAnimationEase);
                selectedCard.transform.DORotate(originalCardRotation.eulerAngles, cardCancelAnimationTime);
            });
            sequence.OnComplete(() =>
            {
                selectedCard.GetComponent<SortingGroup>().sortingOrder = originalCardSortingOrder;
                selectedCard = null;
            });
        }
    }
    
    private void UpdateSelectedCard()
    {
        if (selectedCard != null)
        {
            var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            selectedCard.transform.position = mousePosition;
        }
    }
    
}
