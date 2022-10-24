using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Surfer.Input;
using UnityEngine.Events;
using FMODUnity;
public class MenuButtons : MonoBehaviour
{

    public Canvas howtoPlay;
    public Canvas menuCanvas;
    public AudioSource sfx;
    public StudioEventEmitter emitter;

    private void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
        if (howtoPlay)
            howtoPlay.enabled = false;
        if (menuCanvas)
            menuCanvas.enabled = true;

        if (emitter)
            Debug.Log("pog");
    }

    public void ExitGame()
    {
        //sfx.Play();
        Application.Quit();
    }

    public void LoadScene(int sceneIndex)
    {
        //sfx.Play();
        SceneManager.LoadScene(sceneIndex);
    }

    //this function is only used in demos - shows a quick diagram of the controls
    public void StartGame()
    {
        //sfx.Play();
        emitter.Play();
        howtoPlay.enabled = true;
        menuCanvas.enabled = false;
    }
}
