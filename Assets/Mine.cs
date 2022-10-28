using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            other.GetComponent<PlayerTriggerChecker>().RespawnPlayer();
            //Destroy(transform.parent.gameObject);
        }
    }
}
