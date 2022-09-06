using Cysharp.Threading.Tasks;
using UnityEditor.AppleTV;
using UnityEngine;

namespace Surfer.Player.MovementModes
{
    [CreateAssetMenu(fileName = "AdventureMde", menuName = "Surfer/AdventureMode")]
    public class AdventurerMode : MovementMode, IMode
    {
        private Vector2 _currentInputVector;
        private Vector2 _smoothInputVelocity;


        protected override async UniTask OnModeChanged()
        {
            await base.OnModeChanged();
            await DelayModeChange(_cancellationSource.Token);
        }

        protected async void OnModeChanged(Rigidbody rb, CapsuleCollider capsuleCollider)
        {
            rb.useGravity = true;
            rb.angularVelocity = Vector3.zero;
            capsuleCollider.enabled = false;
            await OnModeChanged();
        }

        public void MovePlayer(CharacterController controller, Vector3 direction, float movementSpeed)
        {
            // _currentInputVector = Vector2.SmoothDamp(_currentInputVector, direction, ref _smoothInputVelocity, smoothSpeed, 1 );
            controller.Move(direction * movementSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Allows the player character to jump
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="direction"></param>
        /// <param name="jumpPressed"></param>
        /// <param name="canDoubleJump"></param>
        /// <param name="appliedMovement"></param>
        /// <returns>Returns the updated movementDirection</returns>
        public Vector3 Jump(CharacterController controller, Vector3 direction,  bool jumpPressed,bool canDoubleJump ,ref Vector3 appliedMovement)
        {
            if (jumpPressed && controller.isGrounded && !_isJumping)
            {
                _currentJumpNumber++;
                _isJumping = true;
                direction.y = _initialJumpVelocity;
                appliedMovement.y = direction.y;
                return direction;
            } 

            if (canDoubleJump && !controller.isGrounded&&  _currentJumpNumber < _numberOfJumps && !controller.isGrounded && jumpPressed)
            {
                _currentJumpNumber++;
                _isJumping = true;
                direction.y = _initialJumpVelocity * _multiJumpModifier;
                appliedMovement.y = direction.y;
                return direction;
            }

            if (!jumpPressed && _isJumping && controller.isGrounded)
                _isJumping = false;

            return direction;
        }

        float IMode.Initialise() => Initialise();
        public float GetFallMultipler() => fallMultipler;
        public float ResetJump() => _currentJumpNumber = 0;



    }
}