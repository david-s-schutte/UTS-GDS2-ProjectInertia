using System;
using UnityEngine;

namespace Surfer.UI
{
    /// <summary>
    /// The base controller class for any UI component in the scene
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public abstract class MonoUI : MonoBehaviour
    {
        internal Canvas canvas;
        
        public GameObject Instance => gameObject;
        
        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        /// <summary>
        /// Updates the order of the UI component (!This should only be determined by the UIManager!)
        /// </summary>
        /// <param name="sortingOrderIndex"></param>
        public void SetSortingOrder(int sortingOrderIndex)
        {
            canvas.sortingOrder = sortingOrderIndex;
        }
        
        /// <summary>
        /// Called when the UI is first registered from UIManager
        /// </summary>
        public virtual void OnRegistered() {}
        
        /// <summary>
        /// Called before the UI is UnRegistered from UIManager
        /// </summary>
        public virtual void OnUnregistered() {}
        
    }
}
