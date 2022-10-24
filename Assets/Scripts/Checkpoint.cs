using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Surfer.Audio;
using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    private PlaySoundOneShot _soundPlayer;

    private void Start()
    {
        _soundPlayer = GetComponent<PlaySoundOneShot>();
        
    }

    public void PlaySound()
    {
        EventInstance instance = _soundPlayer.PlaySoundOnce(_soundPlayer.SelectedTrack);
    }



}
