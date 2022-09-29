using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private ScoreSystem scoreSystem;

    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI currentScore;


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
}
