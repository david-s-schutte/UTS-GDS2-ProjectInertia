namespace Surfer.Audio
{
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

        public void PlaySoundOnce(AudioTrack track) => _audioManager.PlaySoundOnce(track);

        public void PlaySoundOnce(AudioTrack track, float volumeOverride) => _audioManager.PlaySoundOnce(track, volumeOverride);
    }
}
