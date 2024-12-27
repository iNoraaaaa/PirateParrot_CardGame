using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectAction : ScriptableObject
{
    public abstract string GetName();

    public abstract void Execute(GameObject gameObj);
}
