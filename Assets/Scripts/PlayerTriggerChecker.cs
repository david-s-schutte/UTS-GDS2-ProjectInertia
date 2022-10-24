using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerTriggerChecker : MonoBehaviour
{
    [SerializeField] private Vector3 respawnPos;
    [SerializeField] private Camera playerCam;
    bool finishedLevel = false;

    private void Start()
    {
        respawnPos = transform.position;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Checkpoint")
        {
            Transform animator = hit.gameObject.transform.Find("C");
         //   Transform FMOD = hit.gameObject.transform.Find("FMOD");
            // StudioEventEmitter emitter = FMOD.GetComponent<StudioEventEmitter>();
            // StudioGlobalParameterTrigger trigger = FMOD.GetComponent<StudioGlobalParameterTrigger>();
            // //Debug.Log(emitter);
            // emitter.Play();
            // trigger.TriggerParameters();
            animator.GetComponent<Animator>().SetBool("isActivated", true);
            if( respawnPos != hit.gameObject.transform.Find("RespawnPos").position)
            {
                hit.gameObject.GetComponent<Checkpoint>().PlaySound();
                respawnPos = hit.gameObject.transform.Find("RespawnPos").position;
                GameObject.FindWithTag("ScoreManager").GetComponent<ScoreSystem>().AddToScore(60);
            }
        }

        if (hit.gameObject.tag == "KillPlane")
        {
            gameObject.transform.position = respawnPos;
            PlayerController.SetVelocity(Vector3.zero);
            GameObject.FindWithTag("ScoreManager").GetComponent<ScoreSystem>().AddToScore(-60);
        }

        if (hit.gameObject.tag == "JumpPad")
        {
            Transform FMOD = hit.gameObject.transform.Find("FMOD");
            StudioEventEmitter emitter = FMOD.GetComponent<StudioEventEmitter>();
            Debug.Log(emitter);
            emitter.Play();
            
            hit.gameObject.GetComponent<JumpPad>().LaunchPlayer();
            
            if (!hit.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                hit.gameObject.GetComponent<AudioSource>().Play();
                GameObject.FindWithTag("ScoreManager").GetComponent<ScoreSystem>().AddToScore(10);
            }
            //gameObject.transform.Find("Bouncepad").transform.Find("Pattern").gameObject.GetComponent<Animator>().SetBool("isActivated", true);
        }

        if (hit.gameObject.tag == "Goal")
        {
            //very bodge should change later lmao
            if (!finishedLevel)
            {
                GameObject.FindWithTag("ScoreManager").GetComponent<ScoreSystem>().AddToScore(10);
                GameObject.FindWithTag("ScoreManager").GetComponent<ScoreSystem>().EndLevel();
                GameObject.FindWithTag("ScoreManager").GetComponent<ScoreUI>().ShowRankScreen();
                playerCam.gameObject.transform.Find("Music").GetComponent<MusicManagerTemp>().playJingle();
                finishedLevel = true;
            }
        }

        if (hit.gameObject.tag == "BoostPad")
        {
            GameObject.FindWithTag("ScoreManager").GetComponent<ScoreSystem>().AddToScore(20);
        }


    }
}
