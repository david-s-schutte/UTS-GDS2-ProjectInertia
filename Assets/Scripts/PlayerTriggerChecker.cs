using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerChecker : MonoBehaviour
{
    [SerializeField] private Vector3 respawnPos;

    private void Start()
    {
        respawnPos = transform.position;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Checkpoint")
        {
            Transform animator = hit.gameObject.transform.Find("C");
            animator.GetComponent<Animator>().SetBool("isActivated", true);
            respawnPos = hit.gameObject.transform.Find("RespawnPos").position;
        }
        
        if(hit.gameObject.tag == "KillPlane")
        {
            gameObject.transform.position = respawnPos;
            PlayerController.SetVelocity(Vector3.zero);
        }

        if(hit.gameObject.tag == "JumpPad")
        {
            hit.gameObject.GetComponent<JumpPad>().LaunchPlayer();
            gameObject.transform.Find("Bouncepad").transform.Find("Pattern").gameObject.GetComponent<Animator>().SetBool("isActivated", true);
        }

        if(hit.gameObject.tag == "Goal")
        {
            
        }

        if(hit.gameObject.tag == "BoostPad")
        {

        }
    }
}
