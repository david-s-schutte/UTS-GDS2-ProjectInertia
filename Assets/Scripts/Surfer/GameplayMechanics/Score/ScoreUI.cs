using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreUI : MonoBehaviour
{
    private ScoreSystem scoreSystem;

    [SerializeField] private SceneReference _reference;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI rank;

    [SerializeField] private Canvas ingameUI;
    [SerializeField] private Canvas rankUI;


    void Start()
    {
        scoreSystem = GetComponent<ScoreSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentScore();
        UpdateCurrentTimer();
    }

    private void UpdateCurrentScore()
    {
        currentScore.text = "" + scoreSystem.GetCurrentScore();
        //currentScore.text = "0";
    }

    private void UpdateCurrentTimer()
    {
        float currentTime = scoreSystem.GetLevelTime();

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timer.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public async void ShowRankScreen()
    {
        ingameUI.enabled = false;
        rankUI.enabled = true;
        StartCoroutine(LoadSceneDelayCoroutine());
        rank.text = GetComponent<ScoreSystem>().GetFinalRank();
    }
    
    
    //Unfortunate fix to load new scene
    public IEnumerator LoadSceneDelayCoroutine()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(_reference.ScenePath);
    }
}
