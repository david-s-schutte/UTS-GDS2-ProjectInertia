using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Surfer.Game;
using Surfer.Input;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Surfer.UI
{
    public class SceneLoaderUI : MonoUI, IInteractable
    {
        private static readonly int BeginTransition = Animator.StringToHash("BeginTransition");

        [Header("Testing Scene")] [SerializeField]
        private SceneReference _reference;

        [Header("Components")]
        [SerializeField] private GameObject loadingScreenPrefab;
        [SerializeField] private Image progress;
        [SerializeField] private TextMeshProUGUI percentage;
        [SerializeField] private TextMeshProUGUI _pressAnyButtonText;
        [SerializeField] private Animator _animator;

        public PlayerControls Controls { get; set; }

        private SceneLoaderManager loaderManager;
        private PlayerControls _playerControls;
        private bool loadingCompletedFlag = false;

        public void OnEnable()
        {
            DontDestroyOnLoad(gameObject);
            _playerControls = new PlayerControls();

            loaderManager = ManagerLocator.Get<SceneLoaderManager>();

            if (loaderManager != null)
            {
                loaderManager.LoadingStarted += BeginLoadingTransition;
                loaderManager.ProgressUpdated += UpdateLoadingUI;
            }

            if (_animator == null)
                _animator.GetComponent<Animator>();

            loadingScreenPrefab.SetActive(false);
            _playerControls.Enable();
            _pressAnyButtonText.gameObject.SetActive(false);
            
            ManagerLocator.Get<UIManager>().RegisterUI(this,true);
        }

        private void Start()
        {
            InputSystem.onAnyButtonPress.Call(LoadingScreenClosed);
        }
        
        [ContextMenu("TestUIManager")]
        public void TestUIManager()
        {
            ManagerLocator.Get<UIManager>().RegisterUI(this,true);
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            if (loaderManager != null)
            {
                loaderManager.LoadingStarted -= BeginLoadingTransition;
                loaderManager.ProgressUpdated -= UpdateLoadingUI;
            }
            
            _playerControls.Disable();
            ManagerLocator.Get<UIManager>().UnRegisterUI(this);
        }


        private void UpdateLoadingUI(float value)
        {
            progress.fillAmount = value;
        }

        private void BeginLoadingTransition()
        {
            _animator.SetTrigger(BeginTransition);
        }

        private void LoadingCompleted()
        {
            loadingCompletedFlag = true;
            _pressAnyButtonText.gameObject.SetActive(true);
        }

        public void LoadingScreenClosed(InputControl control)
        {
            if (loadingCompletedFlag)
             loadingScreenPrefab.SetActive(false);
        }

        public void TransitionCompleted()
        {
            loadingScreenPrefab.SetActive(true);
            loaderManager.OnTransitionCompleted();
        }


#if UNITY_EDITOR
        public void ForceLoad()
        {
            loaderManager.LoadSceneAsync(_reference);
        }
#endif

       
    }
}