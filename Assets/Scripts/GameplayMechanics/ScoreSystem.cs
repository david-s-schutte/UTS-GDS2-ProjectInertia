using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    //In-game variables
    private float currentScore = 0;             //Stores the current score of the player
    private float currentTrickChain = 0;        //Stores the current trick chain of player as per the TrickSystem
    private float previousTrickChain = 0;       //Stores the trick chain from the last frame
    private float noOfTrickChains = 0;          //Stores the number of individual trick chains the player has racked up
    private float currentFlow = 0;              //Stores the player's current flow as per the TrickSystem
    private bool levelEnded = false;            //Controls whether or not to continue calculating score
    private float levelTimer = 0;               //Stores how much time has been spent in the level

    [Header("Rank Benchmarks")]
    [SerializeField] private float sRankBenchmark;
    [SerializeField] private float aRankBenchmark;
    [SerializeField] private float bRankBenchmark;
    [SerializeField] private float cRankBenchmark;
    [SerializeField] private float dRankBenchmark;

    [Header("Time Bonus Variables")]
    [Tooltip("The maximum time bonus given to the player - default is 10000")]
    [SerializeField] private float maxTimeBonus = 10000;
    [Tooltip("The rate at which the player's time bonus decays - default is 2")]
    [SerializeField] private float timeBonusDecayFactor = 2;
    [Tooltip("The time in seconds when the score system should start decaying the time bonus - default is 30s")]
    [SerializeField] private float startTime = 30;
    [Tooltip("The time in seconds when the score system should set the time bonus to 0 - default is 300s")]
    [SerializeField] private float cutOffTime = 300;

    [Header("Flow Bonus Variables")]
    [Tooltip("The threshold that needs to be crossed to reach Flow Lvl1")]
    [SerializeField] private float flowLvl1Threshold;
    [Tooltip("The multiplier given to the amount of time spent in Lvl1 flow - default is 1.2")]
    [SerializeField] private float flowLvl1BonusMultiplier = 1.2f;
    [Tooltip("The threshold that needs to be crossed to reach Flow Lvl2")]
    [SerializeField] private float flowLvl2Threshold;
    [Tooltip("The multiplier given to the amount of time spent in Lvl2 flow - default is 1.6")]
    [SerializeField] private float flowLvl2BonusMultiplier = 1.6f;
    [Tooltip("The threshold that needs to be crossed to reach Flow Lvl3")]
    [SerializeField] private float flowLvl3Threshold;
    [Tooltip("The multiplier given to the amount of time spent in Lvl2 flow - default is 2")]
    [SerializeField] private float flowLvl3BonusMultiplier = 2;

    [Header("Trick Bonus Variables")]
    [Tooltip("The multiplier given to each trick chain performed - default is 2")]
    [SerializeField] private float trickBonusMultiplier = 2;

    [Header("Penalties")]
    [Tooltip("The amount of points reduced on death - default is 300")]
    [SerializeField] private float deathPenalty = 300;

    private void Update()
    {
        if (!levelEnded)
        {
            levelTimer += Time.deltaTime;

            //Handle the player's trick chain bonus
            /*Need to get the current trick chain from TrickSystem*/
            if (currentTrickChain == 0 && previousTrickChain != 0)
            {
                CalculateTrickChainBonus(previousTrickChain);
                noOfTrickChains++;
            }
            else
                previousTrickChain = currentTrickChain;

            //Handle the player's flow bonus
            /*Need to get the current flow from TrickSystem*/
            CalculateFlowBonus();

            //Handle the palyer's time bonus
            if (levelTimer < startTime)
                return;
            else if (levelTimer > cutOffTime)
                maxTimeBonus = 0;
            else
                DecayTimeBonus();
        }
        else
            CalculateFinalRank(currentScore);
        
    }

    public void CalculateFinalRank(float finalScore)
    {
        if (finalScore >= sRankBenchmark)
            Debug.Log("S Rank!");
        else if (finalScore < sRankBenchmark && finalScore >= aRankBenchmark)
            Debug.Log("A Rank!");
        else if (finalScore < aRankBenchmark && finalScore >= bRankBenchmark)
            Debug.Log("B Rank!");
        else if (finalScore < bRankBenchmark && finalScore >= cRankBenchmark)
            Debug.Log("C Rank!");
        else if (finalScore < cRankBenchmark && finalScore >= dRankBenchmark)
            Debug.Log("D Rank!");
        else
            Debug.Log("E Rank!");
    }

    private void SubtractScore(float scoreToSubtract)
    {
        currentScore -= scoreToSubtract;
    }

    private void DecayTimeBonus()
    {
        maxTimeBonus -= Time.deltaTime * timeBonusDecayFactor;
    }

    private void CalculateTrickChainBonus(float bonusToAdd)
    {
        currentScore += bonusToAdd * trickBonusMultiplier;
    }

    private void CalculateFlowBonus()
    {
        if (currentFlow >= flowLvl3Threshold)
            currentScore += Time.deltaTime * flowLvl3BonusMultiplier;
        else if(currentFlow < flowLvl3Threshold && currentFlow >= flowLvl2Threshold)
            currentScore += Time.deltaTime * flowLvl2BonusMultiplier;
        else if(currentFlow < flowLvl2Threshold && currentFlow >= flowLvl1Threshold)
            currentScore += Time.deltaTime * flowLvl1BonusMultiplier;
    }
}
