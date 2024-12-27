using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI
{
   public CharacterObject Enemy;

   public int PatternIndex;

   public List<Effect> Effects;

   public EnemyAI(CharacterObject enemy)
   {
      Enemy = enemy;
   }
}
