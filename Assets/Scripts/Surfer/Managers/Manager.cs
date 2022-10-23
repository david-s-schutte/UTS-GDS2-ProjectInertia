using UnityEngine;

namespace Surfer.Managers
{
    /// <summary>
    /// Base Manager Class that can be found by the manager locator to statically locate it by other managers. Managers should NOT be in the scene, use a separate script such as a controller to interact with
    /// objects in scene
    /// </summary>
    public abstract class Manager 
    {
        public virtual void ManagerStart() {}
    }
}