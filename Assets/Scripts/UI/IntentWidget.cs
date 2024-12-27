using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntentWidget : MonoBehaviour
{
    [SerializeField] private Image intentImage;
    [SerializeField] private TextMeshProUGUI amountText;

    private const float InitialDelay = 1.25f;
    private const float FadeInDuration = 0.8f;
    private const float FadeOutDuration = 0.5f;

    private void Start()
    {
        var transparentColor = intentImage.color;
        transparentColor.a = 0.0f;
        intentImage.color = transparentColor;
        amountText.color = transparentColor;
    }

    public void OnEnemyTurnBegan()
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(InitialDelay);
        seq.Append(intentImage.DOFade(0.0f, FadeOutDuration));

        seq = DOTween.Sequence();
        seq.AppendInterval(InitialDelay);
        seq.Append(amountText.DOFade(0.0f, FadeOutDuration));
    }

    public void OnIntentChanged(Sprite sprite, int value)
    {
        intentImage.sprite = sprite;
        intentImage.SetNativeSize();
        amountText.text = value.ToString();

        intentImage.DOFade(1.0f, FadeInDuration);
        amountText.DOFade(1.0f, FadeInDuration);
    }

    public void OnHpChanged(int hp)
    {
        if (hp <= 0)
        {
            intentImage.DOFade(0.0f, FadeOutDuration);
            amountText.DOFade(0.0f, FadeOutDuration);
        }
    }
}
