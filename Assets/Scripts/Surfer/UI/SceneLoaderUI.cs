using System;
using System.Security.Cryptography.X509Certificates;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Surfer.Game;

namespace Surfer.UI
{
    public class SceneLoaderUI : MonoBehaviour
    {
        private static readonly int BeginTransition = Animator.StringToHash("BeginTransition");
        
        [Header("Testing Scene")] 
        [SerializeField] private SceneReference _reference;
        [SerializeField] private GameObject loadingScreenPrefab;
        [SerializeField] private Image progress;
        [SerializeField] private TextMeshProUGUI percentage;
        [SerializeField] private Animator _animator;


        private SceneLoaderManager loaderManager;


        public void OnEnable()
        {
            DontDestroyOnLoad(gameObject);

            loaderManager = ManagerLocator.Get<SceneLoaderManager>();

            if (loaderManager != null)
            {
                loaderManager.LoadingStarted += BeginLoadingTransition;
                loaderManager.ProgressUpdated += UpdateLoadingUI;
            }


            if (_animator == null)
                _animator.GetComponent<Animator>();
        }

        private void OnDisable()
        {
            if (loaderManager != null)
            {
                loaderManager.LoadingStarted -= BeginLoadingTransition;
                loaderManager.ProgressUpdated += UpdateLoadingUI;
            }
        }


        private void UpdateLoadingUI(float value)
        {
            progress.fillAmount = value;
        }

        private void BeginLoadingTransition()
        {
            _animator.SetTrigger(BeginTransition);
        }

        private void EndLoadingTransition()
        {
        }


     

        public void TransitionCompleted()
        {
            loaderManager.OnTransitionCompleted();
        }

      //  #if UNITY_EDITOR
        public void ForceLoad()
        {
            loaderManager.LoadSceneAsync(_reference);
        }
      //  #endif
//
        // public void LoadScene()
        // {
        //     StartCoroutine(AsyncLoad((SceneManager.GetActiveScene().buildIndex) + 1));
        // }
        //
        // public void LoadScene(int sceneIndex)
        // {
        //     StartCoroutine((AsyncLoad(sceneIndex)));
        // }
        //
        // IEnumerator AsyncLoad(int sceneIndex)
        // {
        //     AsyncOperation loading = SceneManager.LoadSceneAsync(sceneIndex);
        //     loadingScreen.SetActive(true);
        //
        //     while (!loading.isDone)
        //     {
        //         float amountLoaded = Mathf.Clamp01(loading.progress / 0.9f);
        //         progress.fillAmount = amountLoaded;
        //         percentage.text = amountLoaded * 100f + "%";
        //         yield return null;
        //     }
        // }
    }
}