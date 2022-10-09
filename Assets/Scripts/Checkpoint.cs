using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(hit.gameObject.name);
    }
}
