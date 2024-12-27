using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EndBattlePopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Button endOfGameButton;

    private CanvasGroup canvasGroup;

    private const string VictoryText = "Victory";
    private const string DefeatText = "Defeat";

    private const float FadeInTime = 0.4f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1.0f, FadeInTime);
    }

    public void SetVictoryText()
    {
        titleText.text = VictoryText;
        descriptionText.text = string.Empty;
    }
    
    public void SetDefeatText()
    {
        titleText.text = DefeatText;
        descriptionText.text = string.Empty;
    }

    public void OnEndOfGameButtonPressed()
    {
        EditorApplication.isPlaying = false;
    }
    
    
}
