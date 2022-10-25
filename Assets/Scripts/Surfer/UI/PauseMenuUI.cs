using System;
using Managers;
using Surfer.Input;

namespace Surfer.UI
{
    public class PauseMenuUI : MonoUI, IInteractable
    {
        public PlayerControls Controls { get; set; }


        private UIManager uiManager;


        protected override void OnEnable()
        {
            base.OnEnable();
            uiManager.RegisterUI(this);
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