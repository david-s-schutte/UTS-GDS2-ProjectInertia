using System.Collections;
using System.Collections.Generic;
using Surfer.Managers;
using UnityEngine;

public class AudioManager : Manager
{
   // private static FMOD.Studio.EventInstance _music;
    
    public override void ManagerStart()
    {
        //TODO: Having the music begin on start for now but obviously will need to be changed later. 
        //TODO: We will also need different types of audio controllers to manage 3D sounds since managers arent in the scene
        var music = FMODUnity.RuntimeManager.CreateInstance("event:/background_music");
       music.start();
       // _music.release();
    }
}
