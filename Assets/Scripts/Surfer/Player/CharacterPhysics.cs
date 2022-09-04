using Surfer.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Surfer.Player
{
    [RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
    public class CharacterPhysics : MonoBehaviour
    {
        [Header("Control Variables")] internal Vector3 movementDirection;
        [SerializeField] internal float movementSpeed;
        [SerializeField] internal float jumpHeight;
        [SerializeField] internal float doubleJumpModifier;
        [SerializeField] internal int numberOfJumps;

        [Header("Physics Variables")] [SerializeField]
        private float gravityScale;

        [Header("Component References")] [SerializeField]
        private CharacterController controller;

        private CapsuleCollider _collider;
        private Rigidbody _rb;

        private void OnEnable()
        {
            _collider = GetComponent<CapsuleCollider>();
            _rb = GetComponent<Rigidbody>();
        }

        protected virtual void MoveCharacter(InputAction.CallbackContext ctx)
        {
            ctx.ReadValue<Vector2>();
            MoveCharacter();
        }

        protected virtual void MoveCharacter()
        {
            controller.Move(movementDirection * Time.deltaTime);
        }

        protected virtual void Jump()
        {
        }
    }
}