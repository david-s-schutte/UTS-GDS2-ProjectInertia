using UnityEngine;
using FMODUnity;

namespace Surfer.Audio
{
    [RequireComponent(typeof(BoostPad))]
    public class BoostpadAudio : MonoAudio
    {

        private BoostPad _boostPad;
        private StudioEventEmitter emitter;
        

        void Awake()
        {
            _boostPad = GetComponent<BoostPad>();
            emitter = GetComponent<StudioEventEmitter>(); //we're doing this for now okay
        }
    
        void OnEnable()
        {
            _boostPad.OnTriggeredPad += PlaySound;
        }

        private void OnDisable()
        {
            _boostPad.OnTriggeredPad -= PlaySound;
        }

        public void PlaySound() => emitter.Play();

    }
}
