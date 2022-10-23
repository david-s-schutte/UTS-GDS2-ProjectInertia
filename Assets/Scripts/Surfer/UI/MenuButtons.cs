using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Surfer.Input;
using UnityEngine.Events;

public class MenuButtons : MonoBehaviour
{

    public Canvas howtoPlay;
    public Canvas menuCanvas;
    public AudioSource sfx;

    private void Start()
    {
        if (howtoPlay)
            howtoPlay.enabled = false;
        if (menuCanvas)
            menuCanvas.enabled = true;
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
        howtoPlay.enabled = true;
        menuCanvas.enabled = false;
    }
}
