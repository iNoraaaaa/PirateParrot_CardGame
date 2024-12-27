using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pattern : ScriptableObject
{
   public List<Effect> Effects = new List<Effect>();

   public abstract string GetName();
}
