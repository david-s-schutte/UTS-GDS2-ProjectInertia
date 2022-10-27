using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Surfer.Audio;
using UnityEngine;

[RequireComponent(typeof(ObjectMover), typeof(PlaySoundOneShot))]
public class PlatformAudio : MonoBehaviour
{

    private ObjectMover _platform;
    private PlaySoundOneShot _playSound;
    private Rigidbody _body;

    private EventInstance _instance;

    private void Awake()
    {
        _playSound = GetComponent<PlaySoundOneShot>();
        _platform = GetComponent<ObjectMover>();
        _body.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _instance = _playSound.PlaySoundOnce(_playSound.SelectedTrack);
    }

    private void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_instance,_platform.transform,_body);
    }
}
