using System;
using System.Linq;
using FMODUnity;
using Surfer.Audio;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Surfer.Editor
{
    [CustomEditor(typeof(MonoAudio), true)]
    public class MonoAudioEditor : UnityEditor.Editor
    {
        private SerializedProperty audioList;
        private SerializedProperty key;

        private SerializedProperty _selectedEvent;
        private SerializedProperty _overrideVolume;
        private SerializedProperty _volumeAmount;
        private SerializedProperty _isGlobal;
        private SerializedProperty _playOnAwake;


        [SerializeField] private MonoAudio _monoAudio;

        

        private void OnEnable()
        {
            audioList = serializedObject.FindProperty("Tracks");
            _overrideVolume = serializedObject.FindProperty("overrideVolume");
            _volumeAmount = serializedObject.FindProperty("volumeOverrideAmount");
            _isGlobal = serializedObject.FindProperty("IsGlobal");
            _playOnAwake = serializedObject.FindProperty("PlayOnAwake");

        }

        public override void OnInspectorGUI()
        {
            
            _monoAudio = (MonoAudio) target;
            serializedObject.Update();
          //  EditorGUILayout.PropertyField(audioList, true);

          EditorGUILayout.BeginHorizontal();
          EditorGUILayout.LabelField("Selected Sound", GUILayout.ExpandWidth(false), GUILayout.Width(250));

          if (GUILayout.Button($"{_monoAudio.SelectedTrack.AudioKey}", EditorStyles.popup))
          {
              SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                  new FMODPathProvider(
                      x =>
                      {
                          _monoAudio.SelectedTrack.AudioKey = x.name.Split("/").Last();
                          _monoAudio.SelectedTrack.AudioPath = x.Path;
                          _monoAudio.SelectedItem = x;
                      })
                  {
                      name = null,
                      hideFlags = HideFlags.None
                  });
          }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Audio Key");
            EditorGUILayout.TextField(_monoAudio.SelectedTrack.AudioKey, EditorStyles.textField);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Audio Path");
            EditorGUILayout.TextField(_monoAudio.SelectedTrack.AudioPath, EditorStyles.textField);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_playOnAwake);
            EditorGUILayout.PropertyField(_isGlobal);
            EditorGUILayout.PropertyField(_overrideVolume);
            bool overrideVolumeValue = _overrideVolume.boolValue;
            if (overrideVolumeValue)
                ShowOverrideVolumeSlider();
            
            
            serializedObject.ApplyModifiedProperties();

           // base.OnInspectorGUI();
        }

        private void ShowOverrideVolumeSlider()=> EditorGUILayout.Slider(_volumeAmount, 0f, 1f);
        
    }
}