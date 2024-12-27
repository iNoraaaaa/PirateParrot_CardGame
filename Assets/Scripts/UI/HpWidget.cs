using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpWidget : MonoBehaviour
{
     [SerializeField]
     private Image hpBar;
     [SerializeField]
     private Image hpBarBackground;
     [SerializeField]
     private TextMeshProUGUI hpText;
     [SerializeField]
     private TextMeshProUGUI hpBorderText;

     [SerializeField]
     private GameObject shieldGroup;
     [SerializeField]
     private TextMeshProUGUI shieldText;
     [SerializeField]
     private TextMeshProUGUI shieldBorderText;
     
     private int maxValue;

     public void Initialize(IntVariable hp, int max, IntVariable shield)
     {
          maxValue = max;
          SetHp(hp.Value);
          SetShield(shield.Value);
     }
     
     private void SetShield(int value)
     {
          shieldText.text = $"{value.ToString()}";
          shieldBorderText.text = $"{value.ToString()}";
          SetShieldActive(value > 0);
     }
    
     private void SetShieldActive(bool shieldActive)
     {
          shieldGroup.SetActive(shieldActive);
     }

     private void SetHp(int value)
     {
          var newValue = value / (float)maxValue;

          hpBar.DOFillAmount(newValue, 0.2f).SetEase(Ease.InSine);

          var sequence = DOTween.Sequence();
          sequence.AppendInterval(0.5f);
          sequence.Append(hpBarBackground.DOFillAmount(newValue, 0.2f));
          sequence.SetEase(Ease.InSine);

          hpText.text = $"{value.ToString()} / {maxValue.ToString()}";
          hpBorderText.text = $"{value.ToString()} / {maxValue.ToString()}";
     }

     public void OnHpChanged(int value)
     {
          SetHp(value);
          if (value <= 0)
          {
               gameObject.SetActive(false);
          }
     }

     public void OnShieldChange(int value)
     {
          SetShield(value);
     }
     
}
