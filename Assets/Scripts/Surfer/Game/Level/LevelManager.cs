using System.Collections;
using System.Collections.Generic;
using Surfer.Managers;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This is the base manager class of level manager, this is inherited by its T class parent 
/// </summary>
public abstract class LevelManager : Manager
{
    [Header("Level Details")] 
    [SerializeField] internal string _displayName;
    [SerializeField] internal string _levelDescription;
    
    protected event UnityAction LevelFailed;
    protected event UnityAction ResetToLastCheckpoint;
    protected event UnityAction LevelCompleted;


    public virtual void SavePlayerData() { }
    public virtual void SaveHighscore() { }

}