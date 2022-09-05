using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsSplashScreen : MonoBehaviour
{
    public void StartDemo()
    {
        if (GameObject.Find("In-GameUI").GetComponent<Canvas>().enabled == false)
            GameObject.Find("In-GameUI").GetComponent<Canvas>().enabled = true;
        
        Destroy(gameObject);
    }
}
