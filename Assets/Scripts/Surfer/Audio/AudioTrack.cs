using System;
using UnityEngine;

namespace Surfer.Audio
{
    /// <summary>
    /// Container class to store the key and path used for FMOD
    /// </summary>
    [Serializable]
    public class AudioTrack
    {
        /// <summary>
        /// The string key to reference what audio you are wanting to update (think of it as a shortcut to the audio path)
        /// </summary>
        public string AudioKey;

        /// <summary>
        /// The path to of the audio track from FMOD
        /// </summary>
        public string AudioPath;
    }
}
