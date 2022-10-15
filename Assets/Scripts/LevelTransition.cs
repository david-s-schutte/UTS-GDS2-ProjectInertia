using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    static LevelTransition Instance;
    public Animator transition;
    
    void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public static void LoadLevel(int sceneIndex) {
        if (Instance) {
            Instance.StartLoadCoroutine(sceneIndex);    
        } else {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    void StartLoadCoroutine(int sceneIndex) {
        Instance.StartCoroutine(LoadLevelCoroutine(sceneIndex));
    }

    IEnumerator LoadLevelCoroutine(int sceneIndex)
    {
        transition.SetBool("Loading", true);
        yield return new WaitForSeconds(1f);
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(sceneIndex);
        yield return loadSceneAsync;
        transition.SetBool("Loading", false);
    }
}
