using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManaManager : MonoBehaviour
{
   public IntVariable playerManaVariable;

   private int defaultMana = 3;

   public void SetDefaultMana(int value)
   {
      defaultMana = value;
   }

   public void OnPlayerTurnBegan()
   {
      playerManaVariable.SetValue(defaultMana);
   }
}
