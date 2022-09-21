using UnityEngine;

namespace Surfer.Managers
{
    /// <summary>
    /// Base Manager Class that can be found by the manager locator to statically locate it by other managers
    /// </summary>
    public abstract class Manager : MonoBehaviour
    {
        public virtual void ManagerStart() {}
    }
}