using UnityEngine;

[CreateAssetMenu(
    menuName = "CardGame/Effects/DrawCardEffect",
    fileName = "DrawCardEffect",
    order = 10)]
public class DrawCardEffect : Effect, IEntityEffect
{
    [SerializeField]
    private int cardCount = 1;

    public override string GetName()
    {
        return $"Draw {cardCount} card(s)";
    }

    void IEntityEffect.Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        // Using the CardDeckManager to draw cards
        var deckManager = GameObject.FindObjectOfType<CardDeckManager>();
        if (deckManager != null)
        {
            deckManager.DrawCardsFromDeck(cardCount);
        }
        else
        {
            Debug.LogError("Cannot find CardDeckManager reference!");
        }
    }
}
