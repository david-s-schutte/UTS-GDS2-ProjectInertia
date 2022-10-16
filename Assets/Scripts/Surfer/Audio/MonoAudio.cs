using System;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Config;
using FMOD.Studio;
using Managers;
using UnityEngine;

namespace Surfer.Audio
{
    /// <summary>
    /// The base class of an audio controller 
    /// </summary>
    public class MonoAudio : MonoBehaviour
    {
        internal AudioManager _audioManager;

        protected virtual void Awake()
        {
            _audioManager = ManagerLocator.Get<AudioManager>();
        }
        
     

        public AudioTrack SelectedTrack;
        public ScriptableObject SelectedItem;

        [HideInInspector] public bool PlayOnAwake;
        [HideInInspector] public bool IsGlobal;
        [HideInInspector] public bool overrideVolume = false;
        [HideInInspector] public float volumeOverrideAmount;
    }
}