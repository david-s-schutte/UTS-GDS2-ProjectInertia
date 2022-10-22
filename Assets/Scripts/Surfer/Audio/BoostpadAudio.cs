using UnityEngine;

namespace Surfer.Audio
{
    [RequireComponent(typeof(BoostPad))]
    public class BoostpadAudio : MonoAudio
    {

        private BoostPad _boostPad;

        void Awake()
        {
            _boostPad = GetComponent<BoostPad>();
        }
    
        void OnEnable()
        {
            _boostPad.OnTriggeredPad += PlaySound;
        }

        private void OnDisable()
        {
            _boostPad.OnTriggeredPad -= PlaySound;
        }

        public void PlaySound() => _audioManager.PlaySoundOnce(SelectedTrack, true);

    }
}
