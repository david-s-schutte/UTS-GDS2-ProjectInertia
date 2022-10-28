using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using Surfer.Managers;
using Surfer.Utilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace Surfer.UI
{
    /// <summary>
    /// UIManager is the main handler that holds and processes loading, unloading UI components and handles their layering
    /// </summary>
    public class UIManager : Manager
    {
        //TODO: Probably a Queue would have been better here so a refactor should be done here to make runtime calculations shorter
        public Dictionary<int, MonoUI> UI = new Dictionary<int, MonoUI>();

        /// <summary>
        /// The current UI that is in front of the screen
        /// </summary>
        public MonoUI GetCurrent() => UI[UI.GetLowestKey()];


        /// <summary>
        /// Registers the desired UI, instantiating into the scene and caching it to memory
        /// </summary>
        /// <param name="uiController">The desired UI you want to be instantiated</param>
        /// <param name="bringToFront">Determine if you want the UI to be pushed to front on instantiate (i.e. for pause menu, popups) </param>
        public void RegisterUI<T>(T uiController, bool bringToFront = false) where T : MonoUI
        {
            int maxValue = 0;

            if (UI.Count <= 0)
            {
                UI.Add(0, uiController);
            }
            else
            {
                maxValue = UI.GetHighestKey();
                UI.Add(maxValue + 1, uiController);
            }

            uiController.OnRegistered();
            //DebugValues(UI);

            if (bringToFront && UI.Count > 1)
                BringUIToFront(uiController);
        }


        public void UnRegisterUI<T>(T uiController) where T : MonoUI
        {
            Dictionary<int, MonoUI> leftAdjustedDict = new Dictionary<int, MonoUI>();
            Dictionary<int, MonoUI> rightAdjustedDict = new Dictionary<int, MonoUI>();


            int target = 0;

            //Finding the target uiController to unregister
            foreach (int key in UI.Keys)
            {
                if (UI[key] == uiController)
                {
                    target = key;
                    break;
                }
            }

            leftAdjustedDict = UI.Where(x => x.Key < target) as Dictionary<int, MonoUI>;
            rightAdjustedDict = UI.Where(x => x.Key > target) as Dictionary<int, MonoUI>;

            if (leftAdjustedDict != null && rightAdjustedDict != null)
            {
                UI.Remove(target);

                //Simply adds and re-adds the key to shift the value
                rightAdjustedDict.ToList().ForEach(x =>
                {
                    int key = x.Key;

                    if (UI.Remove(key, out MonoUI value))
                        rightAdjustedDict.Add(key - 1, value);
                });

                UI = leftAdjustedDict.Concat(rightAdjustedDict) as Dictionary<int, MonoUI>;
                //DebugValues(UI);
                return;
            }


            //We can assume if the rightAdjustedDict is empty then its at the end, therefore it can be removed safely
            if (UI.Any(x => x.Key == target))
            {
                UI.Remove(target);
                return;
            }

            Debug.LogError($"Warning, the UI {uiController.name} does not exist and cannot be unregistered!");
        }

        /// <summary>
        /// Brings the desired UI to the front layer
        /// </summary>
        /// <param name="uiController"> The UI Controller to be moved to the front</param>
        public void BringUIToFront<T>(T uiController) where T : MonoUI
        {
            //TODO: Can definitely be optimised so if we are experiencing frame issues update this
            //TODO: Decouple the searches into helper functions
            Dictionary<int, MonoUI> adjustedDict = new Dictionary<int, MonoUI>();

            //TODO: This is used in other functions can probably decouple it
            foreach (int key in UI.Keys)
            {
                if (UI[key] == uiController)
                {
                    UI.Remove(key);
                    break;
                }
            }

            adjustedDict.Add(0, uiController);

            int length = UI.Count;
            for (int i = 0; i < length; i++)
            {
                var lowestValue = UI.Aggregate((x, y) => x.Key < y.Key ? x : y).Key;

                MonoUI currentUI = null;
                if (UI.TryGetValue(lowestValue, out MonoUI ui))
                {
                    currentUI = ui;
                }
                else
                    continue;

                adjustedDict.Add(i + 1, currentUI);
                UI.Remove(lowestValue);
            }

            //DebugValues(adjustedDict);
            ForceUpdateUI();
            UI = adjustedDict;

            //Makes sure that any layer changes done are properly updated during runtime
            ForceUpdateUI();
        }


        public void BringUIToBack<T>(T uiController) where T : MonoUI
        {
            Dictionary<int, MonoUI> adjustedDict = new Dictionary<int, MonoUI>();

            int highestValue = UI.GetHighestKey();
            MonoUI targetUI = null;

            //Finding the target uiController and removing it
            foreach (int key in UI.Keys)
            {
                if (UI[key] == uiController)
                {
                    targetUI = UI[key];
                    UI.Remove(key);
                    break;
                }
            }

            adjustedDict.Add(highestValue, targetUI);

            int length = UI.Count;
            //Shifting the dictionary forward (ex. Target = 2, 1 => 2 => 3 => 4, Changed To:  1 => 3 => 4 => 2
            for (int i = length; i != 0; i--)
            {
                var currentHighestValue = UI.GetHighestKey();

                MonoUI currentUI = null;
                if (UI.TryGetValue(currentHighestValue, out MonoUI ui))
                {
                    currentUI = ui;
                }
                else
                    continue;

                adjustedDict.Add(i - 1, currentUI);
                UI.Remove(currentHighestValue);
            }

            //DebugValues(adjustedDict);
            ForceUpdateUI();
            UI = adjustedDict;
            ForceUpdateUI();
        }

        public void ForceUpdateUI()
        {
            foreach (var kv in UI)
                kv.Value.SetSortingOrder(kv.Key);
        }


        #region Debug

#if UNITY_EDITOR
        public void DebugValues(Dictionary<int, MonoUI> dict)
        {
            dict.ToList().ForEach(x => { Debug.Log($"Key: {x.Key} | Value: {x.Value}"); });
        }
#endif

        #endregion
    }
}