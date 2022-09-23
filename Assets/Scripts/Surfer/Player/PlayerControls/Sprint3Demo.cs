using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprint3Demo: MonoBehaviour
{
    Vector3 respawnPos;

    private void Start()
    {
        respawnPos = gameObject.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "KillPlane")
        {
            transform.position = respawnPos;
        }

        if(other.tag == "Checkpoint")
        {
            respawnPos = other.transform.position;
        }

        if(other.tag == "Goal")
        {
            Invoke("EndDemo", 4.0f);
        }
    }

    private void EndDemo()
    {
        
        //Time.timeScale = 0.0f;
    }
}
