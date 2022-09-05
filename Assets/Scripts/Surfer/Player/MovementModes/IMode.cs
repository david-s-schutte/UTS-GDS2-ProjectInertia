using UnityEngine;

namespace Surfer.Player.MovementModes
{
    
    public interface IMode
    {
        public abstract void MovePlayer(CharacterController controller, Vector2 direction, float movementSpeed);

        public abstract void Jump();

    }
}
