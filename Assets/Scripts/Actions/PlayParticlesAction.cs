using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Actions/Play Particles Action",
    fileName = "PlayParticlesAction",
    order = 9)]
public class PlayParticlesAction : EffectAction
{
    public ParticleSystem Particles;
    public Vector3 Offset;
    
    public override string GetName()
    {
        return "Play Particles";
    }

    public override void Execute(GameObject gameObj)
    {
        var position = gameObj.transform.position;
        var particles = Instantiate(Particles);
        particles.transform.position = position + Offset;
        particles.Play();

        var autoDestroy = particles.gameObject.AddComponent<AutoDestroy>();
        autoDestroy.Duration = 2.0f;
    }
}
