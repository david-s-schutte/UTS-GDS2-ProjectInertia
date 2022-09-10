using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Surfer.Player.MovementModes
{
    public class MovementMode : ScriptableObject
    {
        [Header("Movement Speed")]
        internal float _movementSpeed = 10;

        
        [Header("Jump Variables")] [SerializeField]
        internal float _maxJumpTime = 0.5f;

        [SerializeField] internal float _maxJumpHeight = 1;
        [SerializeField] internal float _multiJumpModifier;
        [SerializeField] internal int _numberOfJumps;

        [Header("Gravity")] 
        [SerializeField] internal int fallMultipler;


        protected CancellationTokenSource _cancellationSource;

        internal float _initialJumpVelocity;
        internal bool _isJumping = false;
        internal int _currentJumpNumber = 0;

        public UnityAction OnChangedMode;


        private void OnEnable()
        {
            OnChangedMode += UniTask.UnityAction(async () => await OnModeChanged());
        }

        private void OnDisable()
        {
            OnChangedMode -= UniTask.UnityAction(async () => await OnModeChanged());
        }

        protected virtual async UniTask OnModeChanged()
        {
            _cancellationSource = new CancellationTokenSource();
            Initialise();
        }

        protected void CancelDelay() => _cancellationSource.Cancel();

        protected async UniTask DelayModeChange(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(1000, false, PlayerLoopTiming.Update, token);
                Debug.Log("Delay Completed!");
            }
            catch (OperationCanceledException e)
            {
                Debug.Log($"Delay Cancel was successful!: {e}");
            }
        }


        // Returns the gravity to be set to the player
        protected float Initialise()
        {
            float timeToApex = _maxJumpTime / 2;
            var gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
            _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
            return gravity;
        }
    }
}