using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(
    menuName = "CardGame/Templates/Hero",
    fileName = "Hero",
    order = 1)]
public class HeroTemplate : CharacterTemplate
{
    public CardBank StartingDeck;
}
