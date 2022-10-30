using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class JumpPad : MonoBehaviour
{
    [SerializeField] bool ignorePlayerEnterVelocity;
    [SerializeField] bool switchToWalking = true;
    [SerializeField] float strength = 20.0f;
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>()) {
            Transform FMOD = gameObject.transform.Find("FMOD");
            StudioEventEmitter emitter = FMOD.GetComponent<StudioEventEmitter>();
        //Debug.Log(emitter);
     emitter.Play();
        LaunchPlayer();
        }
    }

    public void LaunchPlayer()
    {
        PlayerController.SetWalkingMode();
        if (ignorePlayerEnterVelocity)
        {
            PlayerController.SetVelocity(new(0, strength, 0));
        }
        else
        {
            PlayerController.ApplyVelocity(new(0, strength, 0));
        }

     //   if (!GetComponent<AudioSource>().isPlaying) 
         //   GetComponent<AudioSource>().Play();
    }
}
