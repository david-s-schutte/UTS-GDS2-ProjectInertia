using Surfer.Input;

namespace Surfer.UI
{
    public interface IInteractable
    {
        public PlayerControls Controls { get; set; }

        protected void EnableControls() => Controls.Enable();
        
        protected void DisableControls() => Controls.Disable();
        
    }
}
