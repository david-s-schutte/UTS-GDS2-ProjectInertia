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


        [Header("Physics Variables")] [SerializeField]
        private float gravityScale;

        [Header("Component References")] [SerializeField]
        protected CharacterController _controller;

        private CapsuleCollider _collider;
        private Rigidbody _rb;

        protected virtual void OnEnable()
        {
            _collider = GetComponent<CapsuleCollider>();
            _rb = GetComponent<Rigidbody>();
        }
        
    
    }
}