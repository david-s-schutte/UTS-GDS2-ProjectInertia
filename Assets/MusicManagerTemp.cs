using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerTemp : MonoBehaviour
{
    public AudioClip lvlTrack;
    public AudioClip lvlClear;
    public AudioSource source;

    public void playJingle()
    {
        source.Stop();
        source.clip = lvlClear;
        source.Play();
    }
}
