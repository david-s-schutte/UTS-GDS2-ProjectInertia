using Surfer.UI;
using UnityEditor;
using UnityEngine;

namespace Surfer.Editor
{
    [CustomEditor(typeof(SceneLoaderUI))]
    public class SceneLoaderUIEditor : UnityEditor.Editor
    {
        [SerializeField] private SceneLoaderUI _loader;

        public override void OnInspectorGUI()
        {
            _loader = (SceneLoaderUI) target;

            if (GUILayout.Button("Force Load"))
            {
                _loader.ForceLoad();
            }

            base.OnInspectorGUI();
        }
    }
}