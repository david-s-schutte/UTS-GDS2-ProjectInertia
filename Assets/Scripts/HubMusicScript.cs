using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HubMusicScript : MonoBehaviour
{
    public static bool isOutsideHub = true;
    GameObject player;
    StudioGlobalParameterTrigger paramTrigger;
    // Start is called before the first frame update
    void Start()
    {
        paramTrigger = GetComponent<StudioGlobalParameterTrigger>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isOutsideHub)
        {
            paramTrigger.Value = 1;
        }
        else
        {
            paramTrigger.Value = 0;
        }
    }
}
