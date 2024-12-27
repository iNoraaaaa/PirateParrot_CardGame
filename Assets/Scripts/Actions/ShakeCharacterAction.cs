using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


[CreateAssetMenu(
    menuName = "CardGame/Actions/Shake Character Action",
    fileName = "ShakeCharacterAction",
    order = 10)]

public class ShakeCharacterAction : EffectAction
{
    public float Duration;
    public Vector3 Strength;
    
    public override string GetName()
    {
        return "Shake Character";
    }

    public override void Execute(GameObject gameObj)
    {
        gameObj.transform.DOShakePosition(Duration, Strength);
    }
}
