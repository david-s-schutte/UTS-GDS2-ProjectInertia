using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterData
{
    public string ProfileName { get; set; }
    public int Time { get; set; } // should be saved in milliseconds or an easy convertible number 
    public int Points { get; set; } //TODO: Change to points per level (Might need to be stored a level data reference instead im not sure)
}
