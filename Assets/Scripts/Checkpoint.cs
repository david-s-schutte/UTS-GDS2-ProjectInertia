using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using FMOD;


public class Checkpoint : MonoBehaviour
{
    FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        emitter.Play();
        Debug.Log(collision.gameObject.name);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(hit.gameObject.name);
    }
}
