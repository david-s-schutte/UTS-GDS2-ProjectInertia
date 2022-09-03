using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TrickType
{
    Normal,
    Held,
    Transfer
}

[CreateAssetMenu]
public class Trick : ScriptableObject
{
    // Start is called before the first frame update
    [Header("Trick Name")]
    public string TrickName;
    
    [Header("Trick Type")]
    public TrickType Type;

    [Header("Trick Attributes")] 
    public float BaseScore;

    public AnimationClip TrickAnim;


}
