using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public List<EffectActionGroupManager> SourceActions = new List<EffectActionGroupManager>();
    public List<EffectActionGroupManager> TargetActions = new List<EffectActionGroupManager>();
    
    public abstract string GetName();
}
