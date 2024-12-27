using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardUtils
{
   public static bool CardCanBePlayed(CardTemplate card, IntVariable playerMana)
   {
      return card.Cost <= playerMana.Value;
   }
   
   public static bool CardHasTargetableEffect(CardTemplate card)
   {
      // 判断卡牌是否要展示攻击箭头，判断标准是卡牌是否含有针对攻击敌人的特效，这是一个很必要的函数
      foreach (var effect in card.Effects)
      {
         if (effect is TargetableEffect targetableEffect)
         {
            if (targetableEffect.Target == EffectTargetType.TargetEnemy)
            {
               return true;
            }
         }
      }

      return false;

   }
}
