using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Surfer.Player.MovementModes
{
    [CreateAssetMenu(fileName = "AdventureMde",menuName = "Surfer/AdventureMode")]
    public class AdventurerMode : MovementMode, IMode
    {
        [SerializeField] internal float doubleJumpModifier;
        [SerializeField] internal int numberOfJumps;
    
    
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

        public void MovePlayer(CharacterController controller, Vector2 direction, float movementSpeed)
        {
            controller.Move(new Vector3(direction.x,controller.velocity.y,direction.y) * movementSpeed * Time.deltaTime);
            Debug.Log("Adventure mode");
        }

        public void Jump()
        {
            
            
            
        }
    }
} 