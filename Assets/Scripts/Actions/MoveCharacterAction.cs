using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Actions/Move Character Action",
    fileName = "MoveCharacterAction",
    order = 7)]
public class MoveCharacterAction : EffectAction
{
    public float Duration;
    public Vector3 Offset;
    
    public override string GetName()
    {
        return "Move Character";
    }

    public override void Execute(GameObject gameObj)
    {
        var originalPosition = gameObj.transform.position;
        var sequence = DOTween.Sequence();
        sequence.Append(gameObj.transform.DOMove(originalPosition + Offset, Duration));
        sequence.Append(gameObj.transform.DOMove(originalPosition, Duration * 2));
    }
}
