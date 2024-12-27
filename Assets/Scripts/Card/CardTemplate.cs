using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "CardGame/Templates/Card", order = 0)]
public class CardTemplate : ScriptableObject
{
    public int Id;
    public string Name;
    public int Cost;
    public Sprite Picture;
    public CardType Type;
    public List<Effect> Effects = new List<Effect>();
}
