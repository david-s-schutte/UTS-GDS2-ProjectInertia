using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        Application.Quit();
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    //this function is only used in demos - shows a quick diagram of the controls
    public void StartGame()
    {
        howtoPlay.enabled = true;
        menuCanvas.enabled = false;
    }
}
