using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrickUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image flow;
    [SerializeField] private Image stage;
    [SerializeField] private TextMeshPro name;
    [SerializeField] private TextMeshPro value;
    public float timeSinceTrick;
    //public float chainTimeLimit;
    [SerializeField] private Sprite[] stageLabels;
    public float[] trickStage;
    private int currentStage = 0;
    public float score;
    public float decrementAmount;
    public float fillspeed;
    public float bufferamounts;
    
    // Update is called once per frame
    void Update()
    {
        
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

    public void UpdateScore(Trick trick)
    {
        timeSinceTrick = 0f;
        score += trick.BaseScore;
        if (score > trickStage[currentStage])
        {
            if (currentStage < trickStage.Length - 1)
            {
                score -= (trickStage[currentStage]- bufferamounts);
                currentStage++;
                StageChange();
            }
        }

        name.text = trick.name;
        value.text = trick.BaseScore.ToString();
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
