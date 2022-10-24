using System;
using Managers;
using Surfer.Input;
using Surfer.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
namespace Surfer.UI
{
    /// <summary>
    /// An example script on how a UI controller should be setup, and how it work in conjunction with the UIManager
    /// </summary>
    public class PauseMenu : MonoUI, IInteractable
    {
        //Inherited from IInteractable  
        public PlayerControls Controls { get; set; }
        public static bool isPaused;
        

        private void OnDisable()
        {
            _uiManager.UnRegisterUI(this);
        }

        protected override void OnEnable()
        {
            _uiManager.RegisterUI(this, false);
            base.OnEnable();
            EnableControls();
        }

        [ContextMenu("Bring To Back")]
        public void BringToBack()
        {
            _uiManager.BringUIToBack(this);
        }

        [ContextMenu("Bring to Front")]
        public void BringToFront()
        {
            _uiManager.BringUIToFront(this);
        }

        protected override void OnInitialised()
        {
            //Gets the UIManager and registers this MonoUI, you can set the second parameter to true if you want it to immediately go to front on registration
            // Registration simply refers to the UIManager caching a UIController so that it can update the layering for it.
            //This process should be done onEnable
            _uiManager.RegisterUI(this, false);

        }

        public override void OnFront()
        {
            isPaused = true;
            EnableControls();
        }

        //This is an event function that occurs once the UIManager successfully registers the ui controller
        public override void OnRegistered()
        {
            //Here we are simply adding it to front once it is registered to avoid race conditions
            _uiManager.BringUIToFront(this);
        }

        public override void OnUnregistered()
        {
            //Here we are simply destorying the gameobject once it is successfuly deregistered with the UIManager...
            // To ensure that the UIManager has properly updated.
            // A UI controller should not be destroyed BEFORE it is unregistered from the UIManager to ensure the manager is being updated succesfully
            Destroy(gameObject);
        }

        #region IInteractable

        /// <summary>
        /// Enables the new input system (Must be called onFront)
        /// </summary>
        public void EnableControls()
        {
            Controls = new PlayerControls();
            Controls.Enable();
        }

        //Disables the new input system (Must be called if UI is disabled or moved away from front)
        public void DisableControls() => Controls.Disable();

        public void Pause(InputAction.CallbackContext context)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                BringToFront();
            }
            else
            {
                BringToBack();
            }
        }

        public void Resume()
        {
            Debug.Log("hello???");
            BringToBack();
            DisableControls();
        }

        public void Restart()
        {
        }
        public void ReturnToMenu()
        {
          
        }

        public void ReturnToHub()
        {
            
        }
        #endregion
    }
}
#endif