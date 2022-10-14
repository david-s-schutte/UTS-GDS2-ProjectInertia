using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Surfer.Managers;
using UnityEngine;

namespace Surfer.Audio
{
    [Serializable]
    public class AudioList
    {
        // [SerializeField] public List<AudioTrack> AudioTracks;
        //
        // internal AudioManager _audioManager;
        //
        // AudioList() => _audioManager = ManagerLocator.Get<AudioManager>();
        //
        // /// <summary>
        // /// Plays the desired audio from the audio list from a given key
        // /// </summary>
        // /// <param name="targetKey"></param>
        // internal void PlayAudioOnceFromKey(string targetKey)
        // {
        //     AudioTrack track = AudioTracks.First(x => x.AudioKey == targetKey);
        //     _audioManager.PlaySoundOnce(track);
        // }
        //
        // /// <summary>
        // /// Plays the desired audio from the audio list from a given path
        // /// </summary>
        // /// <param name="targetPath"></param>
        // internal void PlayAudioOnceFromPath(string targetPath)
        // {
        //     AudioTrack track = AudioTracks.First(x => x.AudioPath == targetPath);
        //     _audioManager.PlaySoundOnce(track);
        // }
        //
        // /// <summary>
        // /// Plays the desired audio from the audio list from a given AudioTrack class
        // /// </summary>
        // /// <param name="track"></param>
        // internal void PlayAudioOnce(AudioTrack track) => _audioManager.PlaySoundOnce(track);
        //
        // /// <summary>
        // /// Plays the desired song from the audio list from a given key
        // /// </summary>
        // /// <param name="targetKey"></param>
        // internal void PlaySongFromKey(string targetKey)
        // {
        //     AudioTrack track = AudioTracks.First(x => x.AudioKey == targetKey);
        //     _audioManager.PlayBackgroundMusic(track);
        // }
        //
        // /// <summary>
        // /// Plays the desired song from the audio list from a given path
        // /// </summary>
        // /// <param name="targetPath"></param>
        // internal void PlaySongFromPath(string targetPath)
        // {
        //     AudioTrack track = AudioTracks.First(x => x.AudioPath == targetPath);
        //     _audioManager.PlayBackgroundMusic(track);
        // }
        //
        // /// <summary>
        // /// Plays the desired song from the audio list from a given AudioTrack class
        // /// </summary>
        // /// <param name="targetTrack"></param>
        // internal void PlaySongFromTrack(AudioTrack targetTrack) => _audioManager.PlayBackgroundMusic(targetTrack);
    }
}
