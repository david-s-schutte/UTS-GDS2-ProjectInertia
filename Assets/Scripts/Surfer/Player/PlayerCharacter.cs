using System;

namespace Surfer.Player
{
   public class PlayerCharacter : Character<PlayerData>
   {
      private void OnEnable()
      {
         data.Points = 100;
      }
   }
}
