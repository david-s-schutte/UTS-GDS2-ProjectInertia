using UnityEngine;
///CREDIT: Jack Coggins for letting use his architecture for scene referencing

/// <summary>
/// A wrapper containing all the scene reference data a certain level needs on startup (such as object spawns, player spawn, player data etc.)
/// </summary>
public abstract class LevelSceneReference : ScriptableObject
{
   /// <summary>
   /// Contains the consistent path of a scene. (i.e. SceneManager.LoadScene(Scene.ScenePath)
   /// </summary>
   public SceneReference Scene;

}
