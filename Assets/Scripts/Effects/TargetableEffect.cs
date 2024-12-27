using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetableEffect : Effect
{
   public EffectTargetType Target;

   public abstract void Resolve(RuntimeCharacter source, RuntimeCharacter target);
}
