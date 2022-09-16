using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PlasticGui.WorkspaceWindow.PendingChanges;
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

        [SerializeField] private List<MovementMode> _modes;

        [Header("Camera Reference")] [SerializeField]
        private Transform _camera;

        private ReadOnlyCollection<MovementMode> _movementModes;
        private PlayerControls _controls;
        private PlayerState _currentState;
        private int _modeIndex;
        private bool _isMoving = false;
        private bool _isJumping = false;
        private Vector3 _cameraRelativeMovement;
        private bool canJumpAgain = false; //refers for multiple jumps

        /*ADDITIONS MADE BY DAVID IM SORRY FRANCISCO*/
        private Vector3 relativeForward; //store the relative forward direction
        private bool isAutoMove; //bool that controls whether to move the player forward automatically
        private InputAction switchedModes; //

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

            //Added by David
            isAutoMove = true;
            switchedModes = _controls.Player.ChangeMode;
            switchedModes.Enable();
            switchedModes.performed += SwitchAutoMove;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _controls.Enable();
            RegisterInputs();
            _modeIndex = 0;
            _movementModes = _modes.AsReadOnly();
            gravityScale = CurrentMode.Initialise();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        private void RegisterInputs()
        {
            _controls.Player.Move.canceled += MoveCancelled;
            // _controls.Player.Move.performed += MovePlayer;
            _controls.Player.Jump.started += Jump;
            _controls.Player.Jump.canceled += Jump;
            _controls.Player.ChangeMode.started += ChangeMode;
        }

        private void FixedUpdate()
        {
            // if (_isMoving)
            //     _cameraRelativeMovement =  RotateCharacterWithCamera(_camera);

            //   RotateCharacterWithCamera(_camera);
            //  
            UpdateCameraRelativeMovement();
            relativeForward = new Vector3(gameObject.transform.forward.x,
                /*_cameraRelativeMovement.y*/
                gameObject.transform.forward.y
                , gameObject.transform.forward.z);

            if(isAutoMove)
                CurrentMode.MovePlayer(_controller,
                    new Vector3(movementDirection.x, _cameraRelativeMovement.y, movementDirection.z));
            else
                CurrentMode.MovePlayer(_controller,
                    new Vector3(relativeForward.x, relativeForward.y, relativeForward.z));

            UpdateGravity();

            movementDirection = CurrentMode.Jump(_controller,
                new Vector3(movementDirection.x, _cameraRelativeMovement.y, movementDirection.z), _isJumping,
                canJumpAgain, ref _cameraRelativeMovement);

            //UpdateCameraRelativeMovement();

            if (_controller.isGrounded)
            {
                CurrentMode.ResetJump();
                canJumpAgain = false;
            }
        }


        //TODO: Would love to get the movement in this callback but not sure how with camera-relative movement
        // public void MovePlayer(InputAction.CallbackContext ctx)
        // {
        //     if (ctx.canceled)
        //         return;
        //
        //     var inputValue = ctx.ReadValue<Vector2>();
        //     movementDirection.x = inputValue.x;
        //     movementDirection.z = inputValue.y;
        //     
        //     Vector3 forward = _camera.forward;
        //     Vector3 right = _camera.right;
        //     forward.y = 0;
        //     right.y = 0;
        //     forward = forward.normalized;
        //     right = right.normalized;
        //
        //     Vector3 forwardRelativeVerticalInput = movementDirection.z * forward;
        //     Vector3 rightRelativeVerticalInput = movementDirection.x * right;
        //
        //     Vector3 cameraRelativeDirection = forwardRelativeVerticalInput + rightRelativeVerticalInput;
        //     movementDirection = new Vector3(cameraRelativeDirection.x,movementDirection.y,cameraRelativeDirection.z);
        //     _cameraRelativeMovement = new Vector3(cameraRelativeDirection.x,movementDirection.y,cameraRelativeDirection.z);
        //     
        //     _isMoving = true;
        // }

        private void UpdateCameraRelativeMovement()
        {
            if (_controls.Player.Move.IsPressed())
            {
                var inputValue = _controls.Player.Move.ReadValue<Vector2>();
                Vector3 forward = _camera.forward;
                Vector3 right = _camera.right;
                forward.y = 0;
                right.y = 0;
                forward = forward.normalized;
                right = right.normalized;


                Vector3 forwardRelativeVerticalInput = inputValue.y * forward;
                Vector3 rightRelativeVerticalInput = inputValue.x * right;

                Vector3 cameraRelativeDirection = forwardRelativeVerticalInput + rightRelativeVerticalInput;
                movementDirection = new Vector3(cameraRelativeDirection.x, movementDirection.y,
                    cameraRelativeDirection.z);
                _cameraRelativeMovement = new Vector3(cameraRelativeDirection.x, movementDirection.y,
                    cameraRelativeDirection.z);
            }
        }

        private void ChangeMode(InputAction.CallbackContext ctx)
        {
            if (_modeIndex == _modes.Capacity - 1)
            {
                _modeIndex = 0;
                return;
            }

            _modeIndex++;
            Debug.Log(isAutoMove);
            CurrentMode.BeginModeChange();
        }


        public void MoveCancelled(InputAction.CallbackContext ctx)
        {
            _isMoving = false;
            movementDirection = new Vector3(0, movementDirection.y, 0);
            _cameraRelativeMovement = new Vector3(0, _cameraRelativeMovement.y, 0);
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            _isJumping = ctx.ReadValueAsButton();

            if (ctx.canceled)
                canJumpAgain = true;
        }

        private void UpdateGravity()
        {
            var isFalling = movementDirection.y <= 0f || !_isJumping;
            var fallMultiplier = CurrentMode.GetFallMultipler();

            if (_controller.isGrounded && _controller.velocity.y < 0)
            {
                movementDirection.y = groundedGravityScale;
                _cameraRelativeMovement.y = groundedGravityScale;
                return;
            }

            float oldYVelocity = movementDirection.y;

            if (isFalling)
            {
                movementDirection.y = movementDirection.y + (gravityScale * fallMultiplier * Time.deltaTime);
                _cameraRelativeMovement.y = Mathf.Max((oldYVelocity + movementDirection.y) * 0.5f, -20f);
            }
            else
            {
                fallMultiplier = 1;
                movementDirection.y = movementDirection.y + (gravityScale * fallMultiplier * Time.deltaTime);
                _cameraRelativeMovement.y = (oldYVelocity + movementDirection.y) * 0.5f;
            }
        }


        /*CUSTOM FUNCTIONS ADDED BY DAVID*/
        public Vector3 GetCameraRelevantInput()
        {
            return _cameraRelativeMovement;
        }

        public void SwitchAutoMove(InputAction.CallbackContext ctx)
        {
            isAutoMove = !isAutoMove;
        }
    }
}