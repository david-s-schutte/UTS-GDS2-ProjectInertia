using UnityEngine;
using UnityEngine.Events;

namespace Surfer.Player.MovementModes
{
    public class MovementMode : MonoBehaviour
    {
        public UnityEvent OnChangedMode;

        public virtual void OnModeChanged()
        {
            OnChangedMode.Invoke();
        }
        
    
    }
}
