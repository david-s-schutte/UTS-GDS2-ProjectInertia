
using Surfer.Player;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Character<T> : CharacterPhysics where T : CharacterData
{
    [SerializeField] protected Transform _playerModel;

    [Header("Rotation"), Tooltip("Determines how fast the player rotates when turning")] [SerializeField]
    internal float rotationSpeed = 1;


    protected T data;


    internal void RotateCharacter(float rotationPerFrame = 1)
    {
        Vector3 positionToLookAt = Vector3.zero;
        positionToLookAt.x = movementDirection.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = movementDirection.z;

        var currentRotation = transform.rotation;
        var targetRotation = Quaternion.LookRotation(positionToLookAt);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationPerFrame * Time.fixedDeltaTime);
    }

    internal Vector3 RotateCharacterWithCamera( Transform cam, float rotationPerFrame = 1)
    {
        return Vector3.zero;
    }
    
    
}