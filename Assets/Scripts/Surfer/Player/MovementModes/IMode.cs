using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Surfer.Player.MovementModes
{
    public interface IMode
    {
        public abstract void MovePlayer(CharacterController controller, Vector3 direction );

        public abstract Vector3 Jump(CharacterController controller, Vector3 direction, bool isJumping,bool canDoubleJump, ref Vector3 appliedMovement);

        public float Initialise();

        public abstract float GetFallMultipler();

        public abstract float ResetJump();

        public virtual async void  BeginModeChange() {}


    }
}