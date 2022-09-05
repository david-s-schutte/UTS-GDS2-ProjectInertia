using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Surfer.Player.MovementModes
{
    public class MovementMode : ScriptableObject
    {
        public UnityAction OnChangedMode;
        
        protected CancellationTokenSource _cancellationSource;

        private void OnEnable() 
        {
            OnChangedMode += UniTask.UnityAction(async() => await OnModeChanged());
        }

        private void OnDisable()
        {
            OnChangedMode -= UniTask.UnityAction(async() => await OnModeChanged());
        }

        protected virtual async UniTask OnModeChanged() => _cancellationSource = new CancellationTokenSource();
        
        protected void CancelDelay() => _cancellationSource.Cancel();

        protected async UniTask DelayModeChange(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(1000, false, PlayerLoopTiming.Update, token);
            }
            catch (OperationCanceledException e)
            {
                Debug.LogWarning($"Warning trying to cancel the delay was unsuccessful");
            }

        }
        
        

        // protected virtual void MovePlayer(CharacterController controller, float direction, float movementSpeed) {}
        
    }
}