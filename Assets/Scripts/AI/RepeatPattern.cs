using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Patterns/Repeat Pattern",
    fileName = "RepeatPattern",
    order = 1)]

public class RepeatPattern : Pattern
{
    public int Times;
    public Sprite Sprite;
    
    public override string GetName()
    {
        return $"Repeat x {Times.ToString()}";
    }
}
