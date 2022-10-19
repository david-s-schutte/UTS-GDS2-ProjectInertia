using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Surfer.Audio;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] public FootstepParameters _footstepParameters;

    private AudioManager _manager;
    private PlaySoundOneShot soundPlayer;

    private void Start()
    {
        soundPlayer = GetComponent<PlaySoundOneShot>();
        _manager = ManagerLocator.Get<AudioManager>();
    }

    public void PlayFootstep() => soundPlayer.PlaySoundOnce(soundPlayer.SelectedTrack);
    
    
    
    
}
