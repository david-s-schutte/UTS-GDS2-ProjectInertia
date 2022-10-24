using System;
using Managers;
using Surfer.Input;
using UnityEngine.InputSystem;

namespace Surfer.UI
{
    public class PauseMenuUI : MonoUI, IInteractable
    {
        public PlayerControls Controls { get; set; }


        private UIManager uiManager;


        protected override void OnEnable()
        {
            _uiManager.RegisterUI(this, false);
            base.OnEnable();
            EnableControls();
        }

        protected override void OnInitialised()
        {
        }

        public override void OnFront()
        {
        }


        public override void OnRegistered()
        {
            uiManager.BringUIToFront(this);
        }

        public override void OnUnregistered()
        {
            throw new NotImplementedException();
        }

        #region IInteractable

        public void EnableControls()
        {
            Controls = new PlayerControls();
            Controls.Enable();
        }

        public void DisableControls()
        {
            Controls.Disable();
        }

       

        #endregion
    }
}