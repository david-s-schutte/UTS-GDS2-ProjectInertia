using System;
using System.Linq;
using FMODUnity;
using Surfer.Audio;
using UnityEditor;
using UnityEngine;

namespace Surfer.Editor
{
    [CustomEditor(typeof(FootstepParameters))]
    public class FootstepAudioParametersEditor : UnityEditor.Editor
    {
        public FootstepParameters parameters;
    
        public SerializedProperty StepDistance;
        public SerializedProperty MaterialTypes;
        public SerializedProperty overrideMaterial; 
    
        private void OnEnable()
        {
            StepDistance = serializedObject.FindProperty("StepDistance");
            MaterialTypes = serializedObject.FindProperty("MaterialTypes");
            overrideMaterial = serializedObject.FindProperty("overrideMaterial");
        }
    
    
        public override void OnInspectorGUI()
        {
            parameters = (FootstepParameters) target;
            string[] materials = new string[] { };
            
            GUILayout.BeginHorizontal();
            EditorGUILayout.Slider(StepDistance, 0, 20);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            GUI.enabled = overrideMaterial.boolValue;
            EditorGUILayout.PropertyField(MaterialTypes);
            GUI.enabled = true;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Auto Add Materials",EditorStyles.miniButtonRight,GUILayout.Width(120f)))
            {
                string guid = AssetDatabase.FindAssets("t:editoreventref").First();
                string path = AssetDatabase.GUIDToAssetPath(guid);
                EventCache cache = AssetDatabase.LoadAssetAtPath<EventCache>(path);
                EditorEventRef targetRef = null;
                //Could be a better search algorithm but since its outside of runtime i dont mind
                foreach (EditorEventRef eventRef in cache.EditorEvents)
                {
                    if (eventRef.name.Contains("Footstep"))
                    {
                        targetRef = eventRef;
                        break;
                    }
                }
                
                materials = targetRef.LocalParameters[0].Labels;
                parameters.UpdateMaterials(materials);
                serializedObject.Update();
            }

            GUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(overrideMaterial);
            serializedObject.ApplyModifiedProperties();
           // base.OnInspectorGUI();
        }
    }
}
