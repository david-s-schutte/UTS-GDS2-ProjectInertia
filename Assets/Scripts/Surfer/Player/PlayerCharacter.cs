using System;

namespace Surfer.Player
{
   public class PlayerCharacter : Character<PlayerData>, ICharacter
   {
       public enum PlayerState
       {
           Grounded,
           InAir,
           Grinding
       };

       public PlayerState _currentState;
       public PlayerState CurrentState => _currentState;


   }
}
