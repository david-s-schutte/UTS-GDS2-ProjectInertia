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
        [SerializeField] private GameObject transitionGameObject;
        [SerializeField] private Image progress;
        [SerializeField] private TextMeshProUGUI percentage;
        [SerializeField] private TextMeshProUGUI _pressAnyButtonText;
        [SerializeField] private Animator _animator;

        public PlayerControls Controls { get; set; }

        private SceneLoaderManager loaderManager;
        private PlayerControls _playerControls;
        private bool loadingCompletedFlag = false;

        public void Test()
        {
            Debug.Log("BUTTON WORKS");
        }
        protected override void OnInitialised()
        {
            DontDestroyOnLoad(gameObject);
            _playerControls = new PlayerControls();

            loaderManager = ManagerLocator.Get<SceneLoaderManager>();

            if (loaderManager != null)
            {
                loaderManager.LoadingStarted += BeginLoadingTransition;
                loaderManager.ProgressUpdated += UpdateLoadingUI;
                loaderManager.LoadingCompleted += LoadingCompleted;
            }

            if (_animator == null)
                _animator.GetComponent<Animator>();

            loadingScreenPrefab.SetActive(false);
            _playerControls.Enable();
            _pressAnyButtonText.gameObject.SetActive(false);
            
            ManagerLocator.Get<UIManager>().RegisterUI(this,false);
        }

        public override void OnFront()
        {
            EnableControls();
        }

        public override void OnRegistered() { }

        public override void OnUnregistered() { }


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
                loaderManager.LoadingCompleted -= LoadingCompleted;
            }
            
            _playerControls.Disable();
            ManagerLocator.Get<UIManager>().UnRegisterUI(this);
        }


        private void UpdateLoadingUI(float value)
        {
            progress.fillAmount = value;
            percentage.text = (value * 100) + "%";
        }

        private void BeginLoadingTransition()
        {
            transitionGameObject.SetActive(true);
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
            transitionGameObject.SetActive(false);
            loadingScreenPrefab.SetActive(true);
            loaderManager.OnTransitionCompleted();
        }
        
        public void EnableControls()
        {
            Controls = new PlayerControls();
            Controls.Enable();
        }

        public void DisableControls() => Controls.Disable();


#if UNITY_EDITOR
        public void ForceLoad()
        {
            _uiManager.BringUIToFront(this);
            loaderManager.LoadSceneAsync(_reference);
        }
#endif

       
    }
}