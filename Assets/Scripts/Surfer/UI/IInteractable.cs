using Codice.CM.Client.Differences.Graphic;
using Surfer.Input;
using UnityEngine;

namespace Surfer.UI
{
    public interface IInteractable 
    {
        public PlayerControls Controls { get; set; }

        public void EnableControls();

        public void DisableControls();


    }
}
