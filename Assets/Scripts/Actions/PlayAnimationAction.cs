using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Actions/Play Animation Action",
    fileName = "PlayAnimationAction",
    order = 8)]
public class PlayAnimationAction : EffectAction
{
    public Animator Animator;
    public Vector3 Offset;
    
    public override string GetName()
    {
        return "Play animation";
    }

    public override void Execute(GameObject gameObj)
    {
        var position = gameObj.transform.position;
        var animator = Instantiate(Animator);
        animator.transform.position = position + Offset;

        var autoDestroy = animator.gameObject.AddComponent<AutoDestroy>();
        autoDestroy.Duration = 2.0f;
    }
}
