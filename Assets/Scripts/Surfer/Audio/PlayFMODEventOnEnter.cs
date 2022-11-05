using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PlayFMODEventOnEnter : MonoBehaviour
{

    /*EXTREMELY SCUFFED - ONLY USED FOR MSD STUDENT SUBMISSION*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            Transform FMOD = gameObject.transform.Find("FMOD");
            StudioEventEmitter emitter = FMOD.GetComponent<StudioEventEmitter>();
            emitter.Play();
        }
    }
}
