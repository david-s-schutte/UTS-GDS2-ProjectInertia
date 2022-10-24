using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class JumpPad : MonoBehaviour
{
    [SerializeField] bool ignorePlayerEnterVelocity;
    [SerializeField] float strength = 20.0f;
    StudioEventEmitter emitter;
    private void Start()
    {
        //Transform FMOD = gameObject.transform.Find("FMOD");
        emitter = GetComponent<StudioEventEmitter>();
       
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>()) {
            
            if (ignorePlayerEnterVelocity) {
                PlayerController.SetVelocity(new(0, strength, 0));
            } else {
                PlayerController.ApplyVelocity(new(0, strength, 0));
            }
        }
    }

    public void LaunchPlayer()
    {
        if (ignorePlayerEnterVelocity)
        {
            PlayerController.SetVelocity(new(0, strength, 0));
        }
        else
        {
            PlayerController.ApplyVelocity(new(0, strength, 0));
        }

        emitter.Play();
    }
}
