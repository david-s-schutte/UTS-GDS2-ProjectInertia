using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Surfer.Audio;
using Surfer.Managers;
using UnityEngine;

public class AudioManager : Manager
{

    private EventInstance _currentBackgroundMusic;
    

    /// <summary>
    /// Plays a music track based on a given AudioTrack class.
    /// </summary>
    /// <param name="track"></param>
    public void PlayBackgroundMusic(AudioTrack track)
    {
        _currentBackgroundMusic = FMODUnity.RuntimeManager.CreateInstance(track.AudioPath);
        _currentBackgroundMusic.start();
    }
    
    /// <summary>
    ///  Plays a music track based on a given AudioTrack class and overrides its current volume
    /// </summary>
    /// <param name="track"></param>
    /// <param name="volumeOverride"></param>
    public void PlayBackgroundMusic(AudioTrack track, float volumeOverride)
    {
        _currentBackgroundMusic = FMODUnity.RuntimeManager.CreateInstance(track.AudioPath);
        _currentBackgroundMusic.start();
        _currentBackgroundMusic.setVolume(volumeOverride);
    }

    /// <summary>
    /// Plays a desired sound effect or music to play once.
    /// </summary>
    /// <param name="track"></param>
    public void PlaySoundOnce(AudioTrack track) => FMODUnity.RuntimeManager.PlayOneShot(track.AudioPath);
    

    /// <summary>
    /// Plays a desired sound effect or music to play once with a desired position for 3D sounds.
    /// </summary>
    /// <param name="track"></param>
    /// <param name="position"></param>
    public void PlaySoundOnce(AudioTrack track, Vector3 position) => FMODUnity.RuntimeManager.PlayOneShot(track.AudioPath,position);
    

    /// <summary>
    /// Stops the current background song from playing.
    /// </summary>
    /// <param name="allowFadeOut"></param>
    public void StopCurrentSong(bool allowFadeOut = false) => _currentBackgroundMusic.stop(!allowFadeOut ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT);
    
    
}
