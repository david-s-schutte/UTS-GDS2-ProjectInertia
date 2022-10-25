using Surfer.GameplayGimmicks;
using UnityEngine;
using FMODUnity;

namespace Surfer.Audio
{
    [RequireComponent(typeof(BoostPad))]
    public class BoostpadAudio : MonoAudio
    {           

        private BoostPad _boostPad;
        private StudioEventEmitter emitter;
        

        protected override void Awake()
        {
            base.Awake();
            _boostPad = GetComponent<BoostPad>();
            _boostPad.OnTriggeredPad += PlaySound;

        }
    
        private void OnDisable()
        {
            _boostPad.OnTriggeredPad -= PlaySound;
        }

        public void PlaySound() => emitter.Play();

    }
}
