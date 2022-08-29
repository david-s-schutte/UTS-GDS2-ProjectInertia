using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public float decrementAmount;
    public float fillspeed;
    public float bufferamounts;
    public bool test;
    
    
    // Update is called once per frame
    void Update()
    {

        if (test)
        {
            UpdateScore("testTrick", 100f);
            test = false;
        }
        
        timeSinceTrick += Time.deltaTime;
        score -= 100f * Time.deltaTime * decrementAmount;
        if (score < 0)
        {
            if (currentStage > 0)
            {
                currentStage--;
                score = trickStage[currentStage];
                StageChange();
            }
            else
            {
                score = 0;
            }
        }
        flow.fillAmount += Lerp(flow.fillAmount,score/trickStage[currentStage]);
        //flow.fillAmount = score / trickStage[currentStage];
    }

    public void UpdateScore(string name, float addScore)
    {
        timeSinceTrick = 0f;
        score += addScore;
        if (score > trickStage[currentStage])
        {
            if (currentStage < trickStage.Length - 1)
            {
                score -= (trickStage[currentStage]- bufferamounts);
                currentStage++;
                StageChange();
            }
        }

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
