using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(
    menuName = "CardGame/Patterns/Random Pattern",
    fileName = "RandomPattern",
    order = 0)]

public class RandomPattern : Pattern
{
    public List<Probability> Probabilities = new List<Probability>(4);    
    
    public override string GetName()
    {
        return "Random Pattern";
    }
}
