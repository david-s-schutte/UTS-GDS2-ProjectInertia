using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int loadScene;
    public bool load = false;
    
    public GameObject loadingScreen;
    public Image progress;
    public TextMeshProUGUI percentage;


    void Update()
    {
        if (load)
        {
            load = false;
            LoadScene(loadScene);
        }
    }

    public void LoadScene()
    {
        StartCoroutine(AsyncLoad((SceneManager.GetActiveScene().buildIndex)+1));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine((AsyncLoad(sceneIndex)));
    }

    IEnumerator AsyncLoad(int sceneIndex)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        
        while (!loading.isDone)
        {
            float amountLoaded = Mathf.Clamp01(loading.progress / 0.9f);
            progress.fillAmount = amountLoaded;
            percentage.text = amountLoaded * 100f + "%";
            yield return null;
        }
        
    }
}
