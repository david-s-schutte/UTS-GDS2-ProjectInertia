using System;
using FMOD.Studio;
using UnityEngine;

namespace Surfer.Audio
{
    [Serializable]
    public class PlaySoundOneShot : MonoAudio
    {
        protected override void Awake()
        {
            base.Awake();

            if (PlayOnAwake)
            {
                if (overrideVolume)
                    PlaySoundOnce(SelectedTrack, volumeOverrideAmount);
                else
                    PlaySoundOnce(SelectedTrack);
            }
        }
        
        internal void OnDisable()
        {
            _audioManager.StopSound(SelectedTrack);
        }

        public EventInstance PlaySoundOnce(AudioTrack track, bool storeReference = false) =>  _audioManager.PlaySoundOnce(track,storeReference);

        

        public void PlaySoundOnce(AudioTrack track, float volumeOverride, bool storeReference = false) => _audioManager.PlaySoundOnce(track, volumeOverride,storeReference);
    }
}
