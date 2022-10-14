using Mono.Cecil.Cil;

namespace Surfer.Audio
{
    public class PlayMusic : MonoAudio
    {
        protected override void Awake()
        {
            base.Awake();

            if (PlayOnAwake)
            {
                if (overrideVolume)
                    PlaySong(SelectedTrack, volumeOverrideAmount);
                else
                    PlaySong(SelectedTrack);
            }
        }


        public void PlaySong(AudioTrack track) => _audioManager.PlayBackgroundMusic(track);

        public void PlaySong(AudioTrack track, float volumeOverride) => _audioManager.PlayBackgroundMusic(track, volumeOverride);
    }
}