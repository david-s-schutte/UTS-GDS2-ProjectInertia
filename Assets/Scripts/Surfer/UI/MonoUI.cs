using System;
using Managers;
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
        internal UIManager _uiManager;

        
        public GameObject Instance => gameObject;

        private bool _initialiseFlag;
        
        protected virtual void Awake()
        {
            _uiManager = ManagerLocator.Get<UIManager>();
            canvas = gameObject.GetComponent<Canvas>();

            if (canvas != null) // Hack to ensure that awake occurs first 
            {
                _initialiseFlag = true;
                OnInitialised();
            } 
            
        }

        protected virtual void OnEnable()
        {
            if (canvas != null && !_initialiseFlag)
                OnInitialised();
            else
                _initialiseFlag = false;
                
        }

        /// <summary>
        /// Updates the order of the UI component (!This should only be determined by the UIManager!)
        /// </summary>
        /// <param name="sortingOrderIndex"></param>
        public void SetSortingOrder(int sortingOrderIndex)
        {
            if (canvas.sortingOrder == sortingOrderIndex) // guard clause to check if they are the same value already, therefore do nothing
                return;
            
            canvas.sortingOrder = sortingOrderIndex;
            
            if (canvas.sortingOrder == 0)
                OnFront();
        }
        
        protected abstract void OnInitialised();
        
        /// <summary>
        /// Called when the UI is moved to the front
        /// </summary>
        public abstract void OnFront();

        /// <summary>
        /// Called when the UI is first registered from UIManager
        /// </summary>
        public abstract void OnRegistered();

        /// <summary>
        /// Called before the UI is UnRegistered from UIManager
        /// </summary>
        public abstract void OnUnregistered();
    }
}
