using System;
using Surfer.Input;
using Surfer.Player.MovementModes;
using UnityEngine;
using UnityEngine.InputSystem;

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

       private MovementMode _currentMode;

       public PlayerState _currentState;
       public PlayerState CurrentState => _currentState;
       
       [Header("Input")] 
       [SerializeField] private PlayerControls _controls;
       
       protected override void MoveCharacter(InputAction.CallbackContext ctx)
       {
           movementDirection = ctx.ReadValue<Vector2>();
           float yStore = movementDirection.y;
           movementDirection.y = yStore;
           base.MoveCharacter(ctx);
       }
   }
}
