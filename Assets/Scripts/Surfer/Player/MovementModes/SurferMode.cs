using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Surfer.Player.MovementModes;
using UnityEngine;


[CreateAssetMenu(fileName = "SurferMode", menuName = "Surfer/SurferMode")]
public class SurferMode : MovementMode, IMode
{
    [Header("Movement")] [SerializeField] private float _smoothDamping;

    [SerializeField, Tooltip("Constant Move-speed")]
    private float _baseMoveSpeed;

    private Vector2 _lastSavedDirection;

    // [SerializeField] private float rotationRate;
    [SerializeField] private float brakeForce;
    [SerializeField] private float ollieHeight;

    [Header("Physics Variables")] [SerializeField]
    private LayerMask playerLayer;

    [SerializeField] private LayerMask groundLayer;

    private bool m_HitDetect;
    RaycastHit m_Hit;
    public float m_MaxDistance;

    private Vector3 _currentInputVector;
    private Vector3 _smoothInputVelocity;


    protected override async UniTask OnModeChanged()
    {
        await base.OnModeChanged();
        await DelayModeChange(_cancellationSource.Token);
    }

    public void MovePlayer(CharacterController controller, Vector3 direction)
    {
        _currentInputVector = Vector3.SmoothDamp(_currentInputVector, direction, ref _smoothInputVelocity,
            _smoothDamping, _movementSpeed);
        Vector3 smoothedDirection = new Vector3(_currentInputVector.x, direction.y, _currentInputVector.z);

        //if (smoothedDirection.x != 0 && smoothedDirection.y != 0)
        //{
        //    smoothedDirection = smoothedDirection.x > smoothedDirection.z
        //        ? new Vector3(smoothedDirection.x, smoothedDirection.y, smoothedDirection.z + _baseMoveSpeed)
        //        : new Vector3(smoothedDirection.x + _baseMoveSpeed, smoothedDirection.y, smoothedDirection.z);
        //}

        controller.Move(
            new Vector3(smoothedDirection.x, smoothedDirection.y, smoothedDirection.z + _baseMoveSpeed) *
            _movementSpeed * Time.deltaTime);
    }

    public Vector3 Jump(CharacterController controller, Vector3 direction, bool jumpPressed, bool canDoubleJump,
        ref Vector3 appliedMovement)
    {
        if (jumpPressed && controller.isGrounded && !_isJumping)
        {
            _currentJumpNumber++;
            _isJumping = true;
            direction.y = _initialJumpVelocity;
            appliedMovement.y = direction.y;
            return direction;
        }

        if (canDoubleJump && !controller.isGrounded && _currentJumpNumber < _numberOfJumps && !controller.isGrounded &&
            jumpPressed)
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

    public float Initialise()
    {
        float timeToApex = _maxJumpTime / 2;
        var gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
        return gravity;
    }

    public float GetFallMultipler() => fallMultipler;

    public float ResetJump() => _currentJumpNumber = 0;

    public virtual async void BeginModeChange()
    {
        await OnModeChanged();
    }
}