using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Surfer.Audio;
using Surfer.Managers;
using UnityEngine;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : Manager
{
    private EventInstance _currentBackgroundMusic;
    private Dictionary<EventInstance, AudioTrack> _currentSounds = new Dictionary<EventInstance, AudioTrack>();

    private EVENT_CALLBACK _eventCallback;
    private EventReference _eventReference;

    private int _soundTracker;

    protected int SoundTracker
    {
        get
        {
            _soundTracker++;
            return _soundTracker - 1;
        }
        set => _soundTracker = value;
    }

    public override void ManagerStart()
    {
        _soundTracker = 0;
        _eventCallback = SoundEventCallback; // subscribing to event callback
        base.ManagerStart();
    }

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
    /// <param name="storeReference">Caches the sound event to be used/removed at a later point, should not be used for short often used sounds like footsteps</param>
    public void PlaySoundOnce(AudioTrack track, bool storeReference = false)
    {
        track.ID = SoundTracker;

        if (storeReference)
        {
            EventInstance instance = FMODUnity.RuntimeManager.PlayOneShot(track.AudioPath);
            instance.setCallback(_eventCallback);
            _currentSounds.Add(instance, track);
        }
        else
            FMODUnity.RuntimeManager.PlayOneShot(track.AudioPath);
    }

    /// <summary>
    /// Plays a desired sound effect or music to play once and overrides the current volume
    /// </summary>
    /// <param name="track"></param>
    /// <param name="volumeOverride"></param>
    public void PlaySoundOnce(AudioTrack track, float volumeOverride, bool storeReference = false)
    {
        if (storeReference)
        {
            EventInstance instance = FMODUnity.RuntimeManager.PlayOneShot(track.AudioPath,volumeOverride);
            instance.setCallback(_eventCallback);
            _currentSounds.Add(instance, track);
        }
        else
            FMODUnity.RuntimeManager.PlayOneShot(track.AudioPath);
    }


    /// <summary>
    /// Plays a desired sound effect or music to play once with a desired position for 3D sounds.
    /// </summary>
    /// <param name="track"></param>
    /// <param name="position"></param>
    public void PlaySoundOnce(AudioTrack track, Vector3 position, bool storeReference = false)
    {
        if (storeReference)
        {
            EventInstance instance = FMODUnity.RuntimeManager.PlayOneShot(track.AudioPath, position);
            instance.setCallback(_eventCallback);
            _currentSounds.Add(instance, track);
        }
        else
            FMODUnity.RuntimeManager.PlayOneShot(track.AudioPath);
    }


    /// <summary>
    /// Stops the current background song from playing.
    /// </summary>
    /// <param name="allowFadeOut"></param>
    public void StopCurrentSong(bool allowFadeOut = false) =>
        _currentBackgroundMusic.stop(!allowFadeOut ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT);

    public void StopSound(AudioTrack track, bool allowFadeOut = false)
    {
        try
        {
            EventInstance targetSoundInstance = _currentSounds.First(x => x.Value.AudioKey == track.AudioKey).Key;
            targetSoundInstance.stop(!allowFadeOut ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Warning, the sound you are trying to find was not found {track.AudioPath}");
        }
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    private FMOD.RESULT SoundEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE callback, IntPtr instancePtr,
        IntPtr parameterPtr)
    {
        EventInstance instance = new EventInstance(instancePtr);

        switch (callback)
        {
            case EVENT_CALLBACK_TYPE.STOPPED:
            {
                var sound = new Sound(parameterPtr);
                var result = sound.release();
                sound.clearHandle();
                result = instance.release();
                instance.clearHandle();
                _currentSounds.Remove(instance);
                break;
            }
        }

        return FMOD.RESULT.OK;
    }
}