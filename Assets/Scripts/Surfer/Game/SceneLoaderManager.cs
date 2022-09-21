using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Surfer.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Surfer.Game
{
    /// <summary>
    /// This is the manager that handles asynchronous scene management, dealing with any transition animations and calling for data to be saved if needed
    /// </summary>
    
    public class SceneLoaderManager : Manager
    {
        protected IProgress<float> _currentProgress;
        
        
        private CancellationTokenSource _sceneToken;

        private void OnEnable()
        { 
            DontDestroyOnLoad(gameObject);

        }


        public async UniTask LoadSceneAsync(SceneReference scene,CancellationToken token)
        {

            _currentProgress = new Progress<float>(x => Debug.Log($"Loading Level: {x}"));                              
            
            try
            {
                await SceneManager.LoadSceneAsync(scene.ScenePath).ToUniTask(progress: _currentProgress,PlayerLoopTiming.Update,token);
               
            }
        }

  
    }
}
