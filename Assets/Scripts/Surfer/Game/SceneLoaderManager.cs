using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Surfer.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Surfer.Game
{
    /// <summary>
    /// This is the manager that handles asynchronous scene management, dealing with any transition animations and calling for data to be saved if needed
    /// </summary>
    public class SceneLoaderManager : Manager
    {
        private CancellationTokenSource _sceneToken;
        private IProgress<float> _currentProgress;
        private float _progressValue;

        private bool _transitionAnimationCompleted = false;
        
        
        public float CurrentProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                ProgressUpdated?.Invoke(CurrentProgressValue);
            }
        }


        #region Events

        public event UnityAction LoadingStarted;
        public event UnityAction LoadingCompleted;
        public event UnityAction<float> ProgressUpdated;

        #endregion

        /// <summary>
        /// To update the load async method to know when to start loading
        /// </summary>
        public void OnTransitionCompleted() => _transitionAnimationCompleted = true;


        /// <summary>
        /// Loads a given scene asynchronously 
        /// </summary>
        /// <param name="scene">A scene reference is a wrapper to a scene, containing a consistent scene path</param>
        /// <param name="token">Cancellation token to stop the scene loading, to cancel the task, call the SceneLoaderManager token source using _sceneToken.Cancel();</param>
        public async UniTask LoadSceneAsync(SceneReference scene, CancellationToken token = default)
        {
            LoadingStarted?.Invoke();

            //Loading should not begin until the transition to the loading screen has begun (We could improve this with some multi-tasking way of doing it but I am unsure of doing this process efficiently)
            await UniTask.WaitUntil(() => _transitionAnimationCompleted, PlayerLoopTiming.Update, token);
            _transitionAnimationCompleted = false;

            //IProgress keeps track of the current loading value
            _currentProgress = new Progress<float>(x =>
            {
                Debug.Log($"Loading Level: {x}");
                CurrentProgressValue = x;
            });

            try
            {
                //We are awaiting the scene manager to load the scene and converting it to the UniTask system
                await SceneManager.LoadSceneAsync(scene.ScenePath)
                    .ToUniTask(progress: _currentProgress, PlayerLoopTiming.Update, token);
                LoadingCompleted?.Invoke();
            }
            catch (OperationCanceledException e) // Catches if the token was cancelled
            {
                Debug.Log($"Scene loading was cancelled {e}");
                _transitionAnimationCompleted = false;
            }
        }
    }
}