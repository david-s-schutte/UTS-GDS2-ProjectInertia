using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sprint3Demo: MonoBehaviour
{
    public Vector3 respawnPos;
    public GameObject player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ResetLevel();
    }

    private void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KillPlane")
        {
            player.transform.position = respawnPos;
            Debug.Log("fucl");
        }

        if (other.tag == "Checkpoint")
        {
            respawnPos = other.transform.position;
        }

        if (other.tag == "Goal")
        {
            Invoke("EndDemo", 2.0f);
        }
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Rigidbody other = hit.collider.attachedRigidbody;

    //    if (other)
    //    {
    //        if (other.tag == "KillPlane")
    //        {
    //            transform.position = respawnPos;
    //        }

    //        if (other.tag == "Checkpoint")
    //        {
    //            respawnPos = other.transform.position;
    //        }

    //        if (other.tag == "Goal")
    //        {
    //            Invoke("EndDemo", 4.0f);
    //        }
    //    }
    //}

    private void EndDemo()
    {
        SceneManager.LoadScene(2);
    }
}
