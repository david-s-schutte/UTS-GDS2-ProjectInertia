using UnityEngine;

namespace Surfer.Game
{
    ///<summary>
    /// To be inherited by the levels respective managers where T is a wrapper for the scene reference
    /// </summary>
    public abstract class LevelManager<T> : LevelManager where T : LevelSceneReference
    {

        public abstract void OnLevelLoaded();
        public abstract void OnLevelUnloaded();
    }
}