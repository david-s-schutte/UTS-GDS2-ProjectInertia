using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using Surfer.Audio;
using Surfer.Managers;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : Manager
{

    private EventInstance _currentBackgroundMusic;
    private Dictionary<AudioTrack,EventInstance> _currentSounds = new Dictionary<AudioTrack, EventInstance>();

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
    public void PlaySoundOnce(AudioTrack track) => _currentSounds.Add(track,FMODUnity.RuntimeManager.PlayOneShot(track.AudioPath));

    /// <summary>
    /// Plays a desired sound effect or music to play once and overrides the current volume
    /// </summary>
    /// <param name="track"></param>
    /// <param name="volumeOverride"></param>
    public void PlaySoundOnce(AudioTrack track, float volumeOverride) => _currentSounds.Add(track,FMODUnity.RuntimeManager.PlayOneShot(track.AudioPath, volumeOverride));
    
    

    /// <summary>
    /// Plays a desired sound effect or music to play once with a desired position for 3D sounds.
    /// </summary>
    /// <param name="track"></param>
    /// <param name="position"></param>
    public void PlaySoundOnce(AudioTrack track, Vector3 position) => _currentSounds.Add(track,FMODUnity.RuntimeManager.PlayOneShot(track.AudioPath,position));
    

    /// <summary>
    /// Stops the current background song from playing.
    /// </summary>
    /// <param name="allowFadeOut"></param>
    public void StopCurrentSong(bool allowFadeOut = false) => _currentBackgroundMusic.stop(!allowFadeOut ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT);

    public void StopSound(AudioTrack track, bool allowFadeOut = false)
    {
        try
        {
            EventInstance targetSoundInstance = _currentSounds.First(x => x.Key.AudioKey == track.AudioKey).Value;
            targetSoundInstance.stop(!allowFadeOut ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Warning, the sound you are trying to find was not found {track.AudioPath}");
        }
    }

    
}
