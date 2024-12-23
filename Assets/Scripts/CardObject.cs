using System.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro costText;
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private TextMeshPro typeText;
    [SerializeField] private TextMeshPro descriptionText;

    [SerializeField] private SpriteRenderer picture;

    public CardTemplate template;
    private RuntimeCard runtimeCard; 

    
    private void Start()
    {
        var testCard = new RuntimeCard
        {
            Template = template
        };
        SetInfo(testCard);
    }

    public void SetInfo(RuntimeCard card)
    {
        runtimeCard = card;
        template = card.Template;
        costText.text = template.Cost.ToString();
        nameText.text = template.Name;
        typeText.text = template.Type.TypeName;
        var builder = new StringBuilder();
        descriptionText.text = builder.ToString();
        picture.sprite = template.Picture;
    }
}
