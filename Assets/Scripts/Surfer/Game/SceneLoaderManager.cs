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
        private IProgress<float> _currentProgress;
        private float _progressValue;
        private CancellationTokenSource _sceneToken;
        private bool _transitionAnimationCompleted = false;

        public float CurrentProgressValue
        {
            get => CurrentProgressValue;
            set
            {
                CurrentProgressValue = value;
                ProgressUpdated?.Invoke(CurrentProgressValue);
            }
        }

        public event UnityAction LoadingStarted;
        public event UnityAction<float> ProgressUpdated;
        
        
        public void OnTransitionCompleted() => _transitionAnimationCompleted = true;
        
        
        /// <summary>
        /// Loads a given scene asynchronously 
        /// </summary>
        /// <param name="scene">A scene reference is a wrapper to a scene, containing a consistent scene path</param>
        /// <param name="token">Cancellation token to stop the scene loading, to cancel the task, call the SceneLoaderManager token source using _sceneToken.Cancel();</param>
        public async UniTask LoadSceneAsync(SceneReference scene,CancellationToken token = default)
        {
            LoadingStarted?.Invoke();
            
            //Loading should not begin until the transition to the loading screen has begun (We could improve this with some multi-tasking way of doing it but I am unsure of doing this process efficiently)
            await UniTask.WaitUntil(() => _transitionAnimationCompleted, PlayerLoopTiming.Update, token);
            
            //IProgress keeps track of the current loading value
            _currentProgress = new Progress<float>(x => 
            {
                Debug.Log($"Loading Level: {x}");
                _progressValue = x;
            });

            try
            {
                //We are awaiting the scene manager to load the scene and converting it to the UniTask system
                await SceneManager.LoadSceneAsync(scene.ScenePath)
                    .ToUniTask(progress: _currentProgress, PlayerLoopTiming.Update, token);
            }
            catch (OperationCanceledException e) // Catches if the token was cancelled
            {
                Debug.Log($"Scene loading was cancelled {e}");
            }
        }
        
        
        
        // using System;
        // using System.Collections;
        // using System.Collections.Generic;
        // using TMPro;
        // using UnityEngine;
        // using UnityEngine.SceneManagement;
        // using UnityEngine.UI;
        //
        // public class SceneLoader : MonoBehaviour
        // {
        //     // Start is called before the first frame update
        //     [SerializeField] private int loadScene;
        //     public bool load = false;
        //     
        //     public GameObject loadingScreen;
        //     public Image progress;
        //     public TextMeshProUGUI percentage;
        //
        //
        //     void Update()
        //     {
        //         if (load)
        //         {
        //             load = false;
        //             LoadScene(loadScene);
        //         }
        //     }
        //
        //     public void LoadScene()
        //     {
        //         StartCoroutine(AsyncLoad((SceneManager.GetActiveScene().buildIndex)+1));
        //     }
        //
        //     public void LoadScene(int sceneIndex)
        //     {
        //         StartCoroutine((AsyncLoad(sceneIndex)));
        //     }
        //
        //     IEnumerator AsyncLoad(int sceneIndex)
        //     {
        //         AsyncOperation loading = SceneManager.LoadSceneAsync(sceneIndex);
        //         loadingScreen.SetActive(true);
        //         
        //         while (!loading.isDone)
        //         {
        //             float amountLoaded = Mathf.Clamp01(loading.progress / 0.9f);
        //             progress.fillAmount = amountLoaded;
        //             percentage.text = amountLoaded * 100f + "%";
        //             yield return null;
        //         }
        //         
        //     }
        // }


  
    }
}
