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


    private void Start()
    {
        if (howtoPlay)
            howtoPlay.enabled = false;
        if (menuCanvas)
            menuCanvas.enabled = true;
    }

    public void ExitGame()
    {
<<<<<<< HEAD
=======
        //sfx.Play();
>>>>>>> Development
        Application.Quit();
    }

    public void LoadScene(int sceneIndex)
    {
<<<<<<< HEAD
=======
        //sfx.Play();
>>>>>>> Development
        SceneManager.LoadScene(sceneIndex);
    }

    //this function is only used in demos - shows a quick diagram of the controls
    public void StartGame()
    {
<<<<<<< HEAD
=======
        //sfx.Play();
>>>>>>> Development
        howtoPlay.enabled = true;
        menuCanvas.enabled = false;
    }
}
