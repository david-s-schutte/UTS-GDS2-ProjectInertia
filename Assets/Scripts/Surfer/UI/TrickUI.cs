using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class TrickUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image flow;
    [SerializeField] private Image stage;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private RectTransform textStart;
    [SerializeField] private RectTransform scoreStart;
    public float timeSinceTrick;
    //public float chainTimeLimit;
    [SerializeField] private Sprite[] stageLabels;
    public float[] trickStage;
    private int currentStage = 0;
    public float score;
    public float currentFill;
    public float decrementAmount;
    public float fillspeed;
    public float bufferamounts;

    private TrickSystem _trickManager;
    

    public float TimeSinceTrick
    {
        get => timeSinceTrick;
        set
        {
            timeSinceTrick = value;
            _trickManager.TimeSinceTrack = timeSinceTrick;
        }
    }

    private void Awake()
    {
        _trickManager = ManagerLocator.Get<TrickSystem>();
    }

    private void OnEnable()
    {
        _trickManager.OnTrickScoreUpdated += UpdateScore;
    }

    private void OnDisable()
    {
        _trickManager.OnTrickScoreUpdated -= UpdateScore;
    }

    // Update is called once per frame
    void Update()
    {
        TimeSinceTrick += Time.deltaTime;
        score -= 100f * Time.deltaTime * decrementAmount;
        /*if (score < 0)
        {
            if (currentStage > 0)
            {
                //currentStage--;
                score = trickStage[currentStage] - Mathf.Abs(score);
                //StageChange();
            }
            else
            {
                score = 0;
            }
        }*/
        //flow.fillAmount += Lerp(flow.fillAmount,score/trickStage[currentStage]);
        //flow.fillAmount = score / trickStage[currentStage];
        currentFill += (score-currentFill) * fillspeed * Time.deltaTime;
        flow.fillAmount = currentFill / trickStage[currentStage];
        if (currentFill > trickStage[currentStage])
        {
            if (currentStage < trickStage.Length - 1)
            {
                currentFill -= (trickStage[currentStage]);
                score -= (trickStage[currentStage]- bufferamounts);
                currentStage++;
                StageChange();
            }
        }
        else if(currentFill < 0)
        {
            if (currentStage > 0)
            {
                currentStage--;
                currentFill = trickStage[currentStage];
                score = trickStage[currentStage] - Mathf.Abs(score);
                StageChange();
            }
            else
            {
                currentFill = 0;
                score = 0;
            }
        }
    }

    public void UpdateScore(string name, float addScore)
    {
        TimeSinceTrick = 0f;
        score += addScore;
        /*if (score > trickStage[currentStage])
        {
            if (currentStage < trickStage.Length - 1)
            {
                score -= (trickStage[currentStage]- bufferamounts);
                //currentStage++;
                //StageChange();
            }
        }*/

        TrickText text = Instantiate(textPrefab,textStart.position,Quaternion.identity, textStart).GetComponent<TrickText>();
        text.CreateText(name);
        text = Instantiate(textPrefab,scoreStart.position,Quaternion.identity,scoreStart).GetComponent<TrickText>();
        text.CreateText("+" + addScore.ToString());
    }

    private void StageChange()
    {
        stage.sprite = stageLabels[currentStage];
    }
    
    private float Lerp(float fill, float frac) 
    {
        return fillspeed * (frac - fill) * Time.deltaTime;
    }
}
