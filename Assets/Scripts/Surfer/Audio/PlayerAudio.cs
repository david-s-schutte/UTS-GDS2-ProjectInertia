using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Managers;
using Surfer.Audio;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] public FootstepParameters _footstepParameters;
    [SerializeField] private PlaySoundOneShot[] soundPlayer; // Scuffed implementation but should work for our purposes

    private readonly float _speedScale = 50f;
    private AudioManager _manager;
    private PlayerController _controller;
    
    private EventInstance windInstance;
    private EventInstance _grindInstance;

    private void Start()
    {
        _manager = ManagerLocator.Get<AudioManager>();
        _controller = GetComponentInParent<PlayerController>();

        if (_controller != null)
        {
            _controller.OnModeChanged += PlayWind;
            _controller.OnGrindStateUpdated += PlayGrindingSound;

        }
    }


    private void OnDisable()
    {
        _controller.OnModeChanged -= PlayWind;
        _controller.OnGrindStateUpdated -= PlayGrindingSound;

    }

    public void PlayWind(float speedValue, bool isSurfer)
    {
        var scaledSpeedValue = speedValue >= _speedScale ? 1f : speedValue / _speedScale;


        if (!isSurfer)
        {
            if (windInstance.isValid())
            {
                windInstance.stop(STOP_MODE.ALLOWFADEOUT);
                windInstance = new EventInstance();
            }

            return;
        }

        if (!windInstance.isValid()) // null checks for event instances
            windInstance = soundPlayer[1].PlaySoundOnce(soundPlayer[1].SelectedTrack);
        
        Debug.Log(scaledSpeedValue);
        windInstance.setParameterByName("Speed", scaledSpeedValue);
    }

    public void PlayGrindingSound(bool grindingStarted)
    {

        if (grindingStarted)
        {
            if (!_grindInstance.isValid())
                _grindInstance = soundPlayer[1].PlaySoundOnce(soundPlayer[2].SelectedTrack);
        }
        else
        {
            if (_grindInstance.isValid())
                _grindInstance.setParameterByNameWithLabel("GrindState","GrindEnded");
        }
        
    }

    public void PlayFootstep() => soundPlayer[0].PlaySoundOnce(soundPlayer[0].SelectedTrack);
}