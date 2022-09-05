using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private PlayerControls _controls;

        [SerializeField] private List<MovementMode> _modes;

        private ReadOnlyCollection<MovementMode> _movementModes;


        private PlayerState _currentState;

        private int _modeIndex;

        public PlayerState CurrentState
        {
            get => _currentState;
            private set => _currentState = value;
        }

        public IMode CurrentMode => _movementModes[_modeIndex] as IMode;

        private void Awake()
        {
            _controls = new PlayerControls();
            movementDirection = Vector2.zero;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _modeIndex = 0;
            _movementModes = _modes.AsReadOnly();
            _controls.Enable();
            RegisterInputs();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        private void RegisterInputs()
        {
            _controls.Player.Move.canceled += MoveCancelled;
            _controls.Player.Move.performed += MovePlayer;
            _controls.Player.Jump.performed += Jump;
        }

        private void Update()
        {
            CurrentMode.MovePlayer(_controller, movementDirection, movementSpeed);
        }


        //called by the new input system
        public void MovePlayer(InputAction.CallbackContext ctx)
        {
            if (ctx.canceled)
                return;
            
            Debug.Log($"Movement direction: {movementDirection}");
            movementDirection = ctx.ReadValue<Vector2>();
        }

        public void MoveCancelled(InputAction.CallbackContext ctx) => movementDirection = Vector2.zero;

        public void Jump(InputAction.CallbackContext ctx)
        {
            CurrentMode.Jump();
        }


        // protected override void MoveCharacter(InputAction.CallbackContext ctx)
        // {
        //     movementDirection = ctx.ReadValue<Vector2>();
        //     float yStore = movementDirection.y;
        //     movementDirection.y = yStore;
        //     base.MoveCharacter(ctx);
        // }
    }
}